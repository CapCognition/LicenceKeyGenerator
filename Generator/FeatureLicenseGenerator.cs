using CapCognition.Licensing;
using CapCognition.Licensing.Security.Cryptography;
using CapCognition.Licensing.Validation;
using System.Xml.Serialization;

namespace LicenceKeyGenerator;

public class FeatureLicenseGenerator
{
    public string? PublicKey { get; private set; }

    public void LoadPrivateKeyFromFile(string privateKeyFilePath)
    {
        using var streamReader = new StreamReader(privateKeyFilePath);
        var serializer = new XmlSerializer(typeof(PPKeyFile));
        var file = (PPKeyFile)serializer.Deserialize(streamReader)!;
        _privateKey = file.PrivateKey;
        PublicKey = file.PublicKey;
    }

    public void GeneratePrivateKeyFile(string keyFilePath, string passphrase)
    {
        var keyGenerator = KeyGenerator.Create();
        var keyPair = keyGenerator.GenerateKeyPair();
        _privateKey = keyPair.ToEncryptedPrivateKeyString(passphrase);
        PublicKey = keyPair.ToPublicKeyString();

        var file = new PPKeyFile
        {
            PublicKey = PublicKey,
            PrivateKey = _privateKey
        };

        using var streamWriter = new StreamWriter(keyFilePath);
        var serializer = new XmlSerializer(typeof(PPKeyFile));
        serializer.Serialize(streamWriter, file);
    }

    public string GenerateLicense(string passphrase, string feature, string version, string? licensedTo, DateTime? licenseExpires, LicenseType licenseType)
    {
        var license = License.New()
            .As(licenseType)
            .WithProductFeature(feature)
            .WithVersion(version)
            .ExpiresAt(licenseExpires)
            .LicensedTo(licensedTo)
            .CreateAndSignWithPrivateKey(_privateKey, passphrase);

        return license.ToBase64String();
    }

    public bool TestLicense(string licenseBase64, bool trial)
    {
        var license = License.Load(licenseBase64);
        var validation = license.Validate()
            .ExpirationDate()
            .And()
            .Signature(PublicKey)
            .AssertValidLicense()
            .ToList();

        if (validation.Count > 0)
        {
            Console.Error.WriteLine($"{validation[0].Message}. {validation[0].HowToResolve}");
            return false;
        }

        if (trial && license.LicenseType != LicenseType.Trial)
        {
            Console.Error.WriteLine("It is not a trial license");
            return false;
        }
        if (!trial && license.LicenseType != LicenseType.Standard)
        {
            Console.Error.WriteLine("It is not a standard license");
            return false;
        }
        return true;
    }

    private string? _privateKey;
}