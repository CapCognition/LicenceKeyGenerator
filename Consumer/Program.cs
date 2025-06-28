using CapCognition.Licensing;
using CapCognition.Licensing.Validation;
using CommandLine;

namespace LicenseKeyConsumer;

//Argument samples:
// - validateLicenseForFeature --featureName "MyFeature" --publicKey thePublicKey --license YourLicenseString

internal class Program
{
    static void Main(string[] args)
    {
        var result = Parser.Default.ParseArguments<ValidateLicenseForFeature>(args);
        result.MapResult(
            (ValidateLicenseForFeature opt) =>
            {
                return ValidateLicenseForFeature(opt.PublicKey!, opt.FeatureName!, opt.License!, opt.Trial);
            },
            errs =>
            {
                Console.Error.WriteLine("Failed to parse command line options");
                return 1;
            });
    }

    private static int ValidateLicenseForFeature(string publicKey, string featureName, string licenseString, bool trial)
    {
        Console.WriteLine($"Validating license for feature '{featureName}' with license '{licenseString}'");

        var license = License.Load(licenseString);
        if (license == null)
        {
            Console.Error.WriteLine("Error: License could not be loaded.");
            return 1;
        }

        var validationErrors = license.Validate()
            .ExpirationDate()
            .And()
            .Signature(publicKey)
            .AssertValidLicense()
            .ToList();

        if (validationErrors.Count > 0)
        {
            Console.Error.WriteLine($"{validationErrors[0].Message}. {validationErrors[0].HowToResolve}");
            return 1;
        }

        if (license.ProductFeature != featureName)
        {
            Console.Error.WriteLine($"Feature {featureName} is not contained in the license");
            return 1;
        }

        if (trial && license.LicenseType != LicenseType.Trial)
        {
            Console.Error.WriteLine("It's not a trial license");
            return 1;
        }
        if (!trial && license.LicenseType != LicenseType.Standard)
        {
            Console.Error.WriteLine("It's not a standard license");
            return 1;
        }

        var licenseParts = license.Version.Split('.');
        var majorVersion = licenseParts.Length >= 1 ? licenseParts[0] : null;
        var minorVersion = licenseParts.Length >= 2 ? licenseParts[1] : null;

        //do your version validation here
        
        Console.WriteLine($"License for {license.CustomerName} loaded and validated");

        return 0;
    }
}