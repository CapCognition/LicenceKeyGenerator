# License Key Generator & Consumer (.NET 8)

This repository contains two .NET 8 console applications for managing feature-based software licensing:

- **LicenceKeyGenerator**: Generates cryptographic key pairs and feature licenses.
- **LicenseKeyConsumer**: Validates feature licenses using a public key.

The license can contain information about: 
- mandatory: the feature
- optional: the version (major/minor)
- optional: the expiration date
- optional: the customer to whom it is licensed
- optional: the type trial or production

---

## Projects

### 1. LicenceKeyGenerator

A command-line tool to:
- Create a new public/private key pair (for signing licenses).
- Generate a license for a specific feature, version, and customer.

#### Usage

**1. Create a private key:**

    createPrivateKey --keyFilePath "private.key" --passphrase mySecretPassphrase

**2. Generate a feature license based on private key:**

    generateFeatureKey --keyFilePath "private.key" --passphrase mySecretPassphrase --featureName "MyFeature" --majorVersion 1 --minorVersion 0 --expirationDate 2025-12-31 --licensedTo "DemoUser" --trial true

#### Options

- `--keyFilePath` (required): Path to the private key file.
- `--passphrase` (required): Passphrase to encrypt/decrypt the private key.
- `--featureName` (required for license): Name of the feature to license.
- `--majorVersion`, `--minorVersion`: Optional version numbers.
- `--expirationDate`: Optional expiration date (YYYY-MM-DD).
- `--licensedTo`: Optional customer name or email.
- `--trial`: Optional if trial license should be produced.

---

### 2. LicenseKeyConsumer

A command-line tool to:
- Validate a license for a specific feature using the public key.

#### Usage

**Validate a license:**

    validateLicenseForFeature --featureName "MyFeature" --publicKey thePublicKey --license YourLicenseString

#### Options

- `--featureName` (required): Name of the feature to validate.
- `--publicKey` (required): Public key for license validation.
- `--license` (required): License string to validate.

---

## Requirements to build and run

- .NET 8 SDK

## Build & Run

##### 1. Restore NuGet packages:

    dotnet restore

##### 2. Build the solution:

    dotnet build


##### 3. Run either project from the command line with the appropriate arguments.




