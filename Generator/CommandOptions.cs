using CommandLine;

namespace LicenceKeyGenerator;

[Verb("createPrivateKey", false, HelpText = "Create a new public and private key pair file")]
public class CreatePrivateKeyOptions
{
    [Option('k',"keyFilePath", HelpText="Path to the key file the should be created (ex. private.key)", Required = true)]
    public string? FilePath { get; set; }

    [Option('p', "passphrase", HelpText = "The passphrase for encrypting the private key", Required = true)]
    public string? Passphrase { get; set; }
}

[Verb("generateFeatureKey", false, HelpText = "Generates a feature key base on the private key")]
public class GenerateFeatureKeyOptions
{
    [Option('k', "keyFilePath", HelpText = "Path to the key file the should be created (ex. private.key)", Required = true)]
    public string? FilePath { get; set; }

    [Option('p', "passphrase", HelpText = "The passphrase for encrypting the private key", Required = true)]
    public string? Passphrase { get; set; }

    [Option('f', "featureName", HelpText = "The the name of the feature to issue the licence", Required = true)]
    public string? FeatureName { get; set; }

    [Option('m', "majorVersion", HelpText = "The optional major version number the license", Required = false)]
    public int? MajorVersion { get; set; }

    [Option('i', "minorVersion", HelpText = "The optional minor version number the license", Required = false)]
    public int? MinorVersion { get; set; }

    [Option('e', "expirationDate", HelpText = "The optional expiration date of the license", Required = false)]
    public DateOnly? ExpirationDate { get; set; } = null;

    [Option('l', "licensedTo", HelpText = "Name or email the feature license is issued to", Required = false)]
    public string? LicensedTo { get; set; } = null;

    [Option('t', "trial", HelpText = "If set, a trial license will be produced instead of a standard license", Required = false, Default = false)]
    public bool Trial { get; set; } = false;
}