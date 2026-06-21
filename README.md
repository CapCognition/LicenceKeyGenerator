# Free License Key SDK for .NET

Free .NET license key SDK for generating, signing and validating software license keys.

This repository provides a free licensing SDK for .NET developers. It includes tools and sample applications for creating cryptographic key pairs, generating signed license keys and validating licenses inside your own applications.

The entire License SDK is free to use. It is not limited to CapCognition products and can be used by any developer for independent software products, SDKs, desktop applications, server applications, internal tools and proof-of-concept licensing workflows.

## What this repository provides

This repository contains a free .NET License SDK and two example console applications:

| Project               | Purpose                                                                                                 |
| --------------------- | ------------------------------------------------------------------------------------------------------- |
| `LicenceKeyGenerator` | Creates cryptographic key pairs and generates signed software license keys.                             |
| `LicenseKeyConsumer`  | Demonstrates how an application can validate a license key for a specific feature using the public key. |

The generator side is intended for the software vendor, application owner or SDK provider.

The consumer side is intended for the application that needs to validate whether a specific feature is licensed.

The SDK itself is free and can be integrated into your own applications without being tied to CapCognition.

## Typical use cases

You can use this free License SDK as a starting point or direct integration for:

* Software license key generation
* Offline license validation
* Feature-based licensing
* Trial license generation
* Production license generation
* Customer-specific license keys
* Version-specific feature activation
* SDK licensing workflows
* Desktop application licensing
* Server application licensing
* Internal enterprise tool licensing
* Commercial software licensing
* Freeware or shareware licensing
* Proof-of-concept licensing systems

## License data

A generated license can contain the following information:

| Field           | Required | Description                                               |
| --------------- | -------: | --------------------------------------------------------- |
| Feature name    |      Yes | The licensed feature or module name.                      |
| Major version   |       No | Optional major version restriction.                       |
| Minor version   |       No | Optional minor version restriction.                       |
| Expiration date |       No | Optional date after which the license is no longer valid. |
| Licensed to     |       No | Optional customer, company, user or email identifier.     |
| Trial flag      |       No | Indicates whether the license is a trial license.         |

This makes it possible to create licenses such as:

* A permanent license for one feature
* A time-limited trial license
* A customer-specific production license
* A license for a specific major/minor version
* A license for an optional paid module

## How it works

The basic workflow is:

1. Generate a private/public key pair.
2. Keep the private key secret.
3. Use the private key to generate signed license keys.
4. Embed or distribute the public key with your application.
5. Validate license keys inside your application with the public key.
6. Enable or disable features based on the validation result.

The important security principle is:

> The private key is used only by the license issuer.
> The public key can be distributed with the application and is used only for validation.

This means your application does not need to contain the private signing key.

## Projects

### 1. LicenceKeyGenerator

`LicenceKeyGenerator` is a command-line tool for creating key pairs and generating feature license keys.

It can be used to:

* Create a new private/public key pair
* Protect the private key with a passphrase
* Generate a license key for a specific feature
* Add optional version information
* Add an optional expiration date
* Add optional customer information
* Mark a license as trial or production

### Create a private key

```bash
dotnet run --project Generator -- createPrivateKey \
  --keyFilePath "private.key" \
  --passphrase "mySecretPassphrase"
```

This creates a private key file that can later be used to generate signed license keys.

Keep this file secure and do not ship it with your application.

### Generate a feature license

```bash
dotnet run --project Generator -- generateFeatureKey \
  --keyFilePath "private.key" \
  --passphrase "mySecretPassphrase" \
  --featureName "MyFeature" \
  --majorVersion 1 \
  --minorVersion 0 \
  --expirationDate 2026-12-31 \
  --licensedTo "DemoCustomer" \
  --trial true
```

This creates a signed license key for the specified feature.

### Generator options

| Option             |                    Required | Description                                                         |
| ------------------ | --------------------------: | ------------------------------------------------------------------- |
| `--keyFilePath`    |                         Yes | Path to the private key file.                                       |
| `--passphrase`     |                         Yes | Passphrase used to encrypt or decrypt the private key.              |
| `--featureName`    | Yes, for license generation | Name of the feature to license.                                     |
| `--majorVersion`   |                          No | Optional major version number.                                      |
| `--minorVersion`   |                          No | Optional minor version number.                                      |
| `--expirationDate` |                          No | Optional expiration date in `YYYY-MM-DD` format.                    |
| `--licensedTo`     |                          No | Optional customer name, company name, user name or email address.   |
| `--trial`          |                          No | Indicates whether the license should be created as a trial license. |

### 2. LicenseKeyConsumer

`LicenseKeyConsumer` is a command-line sample that validates a license key for a specific feature.

It demonstrates the logic that you would normally integrate into your own application.

It can be used to:

* Validate a license key
* Check whether a license belongs to a specific feature
* Validate the license with the public key
* Use the validation result to enable or disable application features

### Validate a license

```bash
dotnet run --project Consumer -- validateLicenseForFeature \
  --featureName "MyFeature" \
  --publicKey "thePublicKey" \
  --license "YourLicenseString"
```

### Consumer options

| Option          | Required | Description                                   |
| --------------- | -------: | --------------------------------------------- |
| `--featureName` |      Yes | Name of the feature that should be validated. |
| `--publicKey`   |      Yes | Public key used for license validation.       |
| `--license`     |      Yes | License string to validate.                   |

## Example scenario

Imagine your application has three optional features:

```text
ExportPdf
AdvancedReporting
ApiAccess
```

You can generate a separate license key for each feature.

For example, to create a trial license for `AdvancedReporting`:

```bash
dotnet run --project Generator -- generateFeatureKey \
  --keyFilePath "private.key" \
  --passphrase "mySecretPassphrase" \
  --featureName "AdvancedReporting" \
  --expirationDate 2026-12-31 \
  --licensedTo "DemoCustomer" \
  --trial true
```

Your application can then validate the license before enabling the feature:

```bash
dotnet run --project Consumer -- validateLicenseForFeature \
  --featureName "AdvancedReporting" \
  --publicKey "thePublicKey" \
  --license "YourLicenseString"
```

If the license is valid, the application can enable the feature. If the license is invalid, expired or created for a different feature, the application can keep the feature disabled.

## Requirements

* .NET 8 SDK
* Windows, Linux or macOS development environment
* Basic command-line knowledge

## Build and run

Clone the repository:

```bash
git clone https://github.com/CapCognition/LicenceKeyGenerator.git
cd LicenceKeyGenerator
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the generator or consumer project with the required command-line arguments.

## Recommended integration pattern

For real applications, a typical setup looks like this:

| Component              | Contains private key? | Contains public key? | Purpose                                             |
| ---------------------- | --------------------: | -------------------: | --------------------------------------------------- |
| License generator      |                   Yes |             Optional | Creates signed license keys.                        |
| Customer application   |                    No |                  Yes | Validates license keys.                             |
| License database / CRM |                    No |             Optional | Stores generated licenses and customer assignments. |
| Build pipeline         |                    No |             Optional | Can package the public key into the application.    |

Never ship the private key with the application that is installed by customers.

## Security notes

This project is intended as a practical licensing SDK and starting point.

When adapting it for production use, consider the following:

* Keep the private key secret.
* Use a strong passphrase for private key protection.
* Do not commit private keys to Git.
* Do not ship private keys with your application.
* Consider using a secure vault for private key storage.
* Protect license generation tools from unauthorized access.
* Log generated licenses and customer assignments.
* Define how lost, leaked or compromised keys are handled.
* Decide whether you need license revocation.
* Decide whether offline-only validation is sufficient for your product.

Offline license validation is useful because it does not require an internet connection, but it also means that revocation is more difficult. If you need immediate license revocation, combine offline validation with an online activation or license status check.

## What this SDK is not

This SDK is not a complete commercial license server.

It does not provide out of the box:

* Online activation
* License revocation service
* Customer portal
* Payment integration
* Subscription billing
* Seat counting
* Floating licenses
* Hardware-bound activation
* Cloud license synchronization

However, it provides a free foundation for generating and validating signed software licenses in .NET applications.

## Suggested extensions

Possible improvements for your own licensing system:

* Add JSON output for generated licenses
* Store generated licenses in a database
* Add customer IDs instead of free-text customer names
* Add product IDs
* Add hardware fingerprinting
* Add online activation
* Add license revocation checks
* Add subscription expiration handling
* Add CLI help output
* Add automated tests
* Add NuGet package support for the consumer validation logic
* Add a small GUI for license generation

## Repository structure

| Path                      | Purpose                                                          |
| ------------------------- | ---------------------------------------------------------------- |
| `LicenceKeyGenerator.sln` | Solution file                                                    |
| `Generator/`              | Console application for key pair creation and license generation |
| `Consumer/`               | Console application for license validation                       |
| `.gitignore`              | Git ignore configuration                                         |
| `README.md`               | Project documentation                                            |

## Contributing

Contributions are welcome.

Useful contributions include:

* Bug fixes
* More examples
* Better command-line help
* Unit tests
* Documentation improvements
* Additional validation scenarios
* Production-hardening suggestions

## License

The License SDK is free for developers.

It can be used independently of CapCognition products and is intended as a free licensing foundation for .NET applications.

Check the repository license file for the exact license terms.
