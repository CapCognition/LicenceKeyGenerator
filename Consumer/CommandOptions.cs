using CommandLine;

namespace LicenseKeyConsumer;

[Verb("validateLicenseForFeature", false, HelpText = "Validates a license for a feature")]
public class ValidateLicenseForFeature
{
    [Option('f', "featureName", HelpText = "The name of the feature to validate", Required = true)]
    public string? FeatureName { get; set; }

    [Option('p', "publicKey", HelpText = "The public key to validate the license", Required = true)]
    public string? PublicKey { get; set; }

    [Option('l', "license", HelpText = "The license to validate", Required = true)]
    public string? License { get; set; }

    [Option('t', "trial", HelpText = "Validate a trial license", Required = false, Default = false)]
    public bool Trial { get; set; }
}
