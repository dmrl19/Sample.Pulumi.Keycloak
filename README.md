# Setup Keycloak with Pulumi

This project is an example on how to setup your keycloak Realm with Pulumi.

## Requirements

* [Install Pulumi](https://www.pulumi.com/docs/get-started/install/)
* [Keycloak](https://www.keycloak.org/documentation)
  * [Keycloak Docker Image](https://www.keycloak.org/server/containers)
* [Smtp docker image](https://github.com/rnwood/smtp4dev)

## What is Keycloak?

Keycloak is a user identity and access management (IAM) solution that helps secure applications and services. It provides features like:

* Authentication: Verifying users identities through various methods like username/password, social login, or multi-factor authentication.
* Authorization: Controlling access to resources based on user roles and permissions.
* Single Sign-On (SSO): Allowing users to log in once and access multiple applications with a single set of credentials.
Identity Federation: Integrating with external identity providers like Google, Facebook, or Active Directory.
* User Management: Managing user accounts, roles, and permissions within the system.

## Setup Locally

Install pulumi and then you can create a project with the command `pulumi new`. You can setup the following environmnet variables to facilite your development:

```bash
export AZURE_STORAGE_ACCOUNT=[NameOfStorageAccount]
export AZURE_STORAGE_KEY=[StorageAccountAzureStorageKey]]
export PULUMI_CONFIG_PASSPHRASE=[PulumiPassphrase]
export PULUMI_STACK=[PulumStack]
```
Then you can setup the pulumi stack `pulumi config set stack dev`.

To install keycloak on your local envrionment, you can run the [docker-compose.yaml](./src/docker-compose.yaml) with the command `docker-compose up`. This will install 2 images, one for keycloak and one for a smtp server to receive emails.