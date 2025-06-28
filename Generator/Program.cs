using CapCognition.Licensing;
using CommandLine;

namespace LicenceKeyGenerator;

//Argument samples:
// - createPrivateKey --keyFilePath "private.key" --passphrase mySecretPassphrase
// - generateFeatureKey --keyFilePath "private.key" --passphrase mySecretPassphrase --featureName "MyFeature" --majorVersion 1 --minorVersion 0 --expirationDate 2025-12-31 --licensedTo "DemoUser" --trial true

internal class Program
{
    static void Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<CreatePrivateKeyOptions, GenerateFeatureKeyOptions>(args);

        result.MapResult(
            (CreatePrivateKeyOptions opt) =>
            {
                CreatePrivateKey(opt.FilePath!, opt.Passphrase!);
                return 0;
            },
            (GenerateFeatureKeyOptions opt) =>
            {
                return IssueFeatureLicense(opt.FilePath!, opt.Passphrase!, opt.FeatureName!, opt.MajorVersion, opt.MinorVersion, opt.ExpirationDate, opt.LicensedTo, opt.Trial);
            },
            errs =>
            {
                Console.Error.WriteLine("Failed to parse command line options");
                return 1;
            });
    }

    private static void CreatePrivateKey(string keyFilePath, string passphrase)
    {
        Console.WriteLine($"Creating public/private key pair file: {keyFilePath}");

        var generator = new FeatureLicenseGenerator();
        generator.GeneratePrivateKeyFile(keyFilePath, passphrase);
        var publicKey = generator.PublicKey!;
        Console.WriteLine($"Key pair file created at '{keyFilePath}' with new public key: {publicKey}");
    }

    private static int IssueFeatureLicense(string keyFilePath, string passphrase, string featureName, int? majorVersion, int? minorVersion, DateOnly? expiration, string? licensedTo, bool trial)
    {
        if (minorVersion.HasValue && !majorVersion.HasValue)
        {
            Console.Error.WriteLine("Error: If minor version is specified, major version must also be specified.");
            return 1;
        }

        var version = majorVersion.HasValue ? majorVersion.ToString() : null;
        if (minorVersion.HasValue)
        {
            version += $".{minorVersion.ToString()}";
        }

        DateTime? expirationDate = null;
        if (expiration.HasValue)
        {
            expirationDate = expiration.Value.ToDateTime(new TimeOnly(0, 0), DateTimeKind.Utc);
        }

        var type = trial ? "trial" : "standard";
        Console.WriteLine($"Generating feature license for '{featureName}' with version '{version}' as {type}...");

        var generator = new FeatureLicenseGenerator();
        generator.LoadPrivateKeyFromFile(keyFilePath);
        var license = generator.GenerateLicense(passphrase, featureName, version!, licensedTo, expirationDate, trial ? LicenseType.Trial : LicenseType.Standard);
        if (string.IsNullOrEmpty(license))
        {
            Console.Error.WriteLine("Error: Failed to generate license.");
            return 1;
        }

        Console.WriteLine($"Validating feature license for '{featureName}'...");
        var validLicense = generator.TestLicense(license, trial);
        if (!validLicense)
        {
            Console.Error.WriteLine("Error: License validation failed.");
            return 1;
        }
        Console.WriteLine($"Feature license for '{featureName}' generated successfully");
        Console.WriteLine(license);
        return 0;
    }
}