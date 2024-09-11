using Pulumi;
using Pulumi.Keycloak;
using Pulumi.Keycloak.OpenId;

public class SampleKeycloakStack : Stack
{
    public SampleKeycloakStack()
    {
        var keycloakProvider = SetupKeycloakProvider();

        var realm = CreateKeycloakRealm(keycloakProvider);
        var client = CreateKeycloakClient(realm, keycloakProvider);
        var role = CreateKeycloakRole(realm, client, keycloakProvider);
        var group = CreateKeycloakGroup(realm, keycloakProvider);
        CreateKeycloakGroupRole(realm, group, role, keycloakProvider);
        CreateKeycloaUserProfile(realm, keycloakProvider);
        CreateClientRoleMapper(realm, client, keycloakProvider);
    }

    /// <summary>
    /// https://github.com/pulumi/pulumi-keycloak?tab=readme-ov-file#configuration
    /// </summary>
    /// <returns></returns>
    private static Provider SetupKeycloakProvider() => new("pulumi-provider", new()
    {
            ClientId = "admin-cli",
            Url = "http://localhost:8080/", // Url where we can access Keycloak
            Username = "admin", // Admin user to access the admin portal
            Password = "admin" // Admin password to access the admin portal
    });

    private static Realm CreateKeycloakRealm(Provider keycloakProvider) => new("new-csharp-realm", new RealmArgs
    {
        RealmName = "test-realm",
        SmtpServer = new Pulumi.Keycloak.Inputs.RealmSmtpServerArgs
        {
            Host = "smtp-server-sample",
            From = "emai-no-reply@email.test"
        }
    }, new()
    {
        Provider = keycloakProvider
    });

    private static Client CreateKeycloakClient(Realm realm, Provider keycloakProvider) => new("client-reseller-portal", new()
    {
        RealmId = realm.Id,
        Name = "reseller-portal",
        ClientId = "reseller-portal",
        AccessType = "CONFIDENTIAL",
        ImplicitFlowEnabled = true,
        Enabled = true,
        ValidRedirectUris = "*",
        StandardFlowEnabled = true,
        DirectAccessGrantsEnabled = true

    }, new()
    {
        Provider = keycloakProvider
    });

    private static Role CreateKeycloakRole(Realm realm, Client client, Provider keycloakProvider) => new Role("role", new()
    {
        RealmId = realm.Id,
        ClientId = client.Id,
        Name = "Sale",
    }, new()
    {
        Provider = keycloakProvider
    });

    private static Group CreateKeycloakGroup(Realm realm, Provider keycloakProvider) => new("group1", new()
    {
        RealmId = realm.Id,
        Name = "Sales"
    }, new()
    {
        Provider = keycloakProvider
    });

    /// <summary>
    /// Creates link between a group and the roles
    /// </summary>
    /// <param name="realm"></param>
    /// <param name="group"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    private static GroupRoles CreateKeycloakGroupRole(Realm realm, Group group, Role role, Provider keycloakProvider) => new("group_roles", new()
    {
        RealmId = realm.Id,
        GroupId = group.Id,
        RoleIds = new[] { role.Id }

    }, new()
    {
        Provider = keycloakProvider
    });

    /// <summary>
    /// Take in consideration, that this will override the default setting, meaning it will delete
    /// the exiting attributes, so be careful with it.
    /// </summary>
    /// <returns></returns>
    private static RealmUserProfile CreateKeycloaUserProfile(Realm realm, Provider keycloakProvider) => new("userprofile", new()
    {
        RealmId = realm.Id,
        Attributes = new[]
            {
            new Pulumi.Keycloak.Inputs.RealmUserProfileAttributeArgs()
            {
                Name= "username",
                DisplayName = "username",
            },
            new Pulumi.Keycloak.Inputs.RealmUserProfileAttributeArgs()
            {
                Name= "email",
                DisplayName = "email",
            },
            new Pulumi.Keycloak.Inputs.RealmUserProfileAttributeArgs()
            {
                Name= "provision",
                DisplayName = "provision",

            },
            new Pulumi.Keycloak.Inputs.RealmUserProfileAttributeArgs()
            {
                Name= "Vp_Number",
                DisplayName = "Vp_Number",
            }
        }
    }, new()
    {
        Provider = keycloakProvider
    });

    /// <summary>
    /// This will create a mapper on the specifie client, within the dedicated client scope
    /// To include the client roles into the claims on the JwT Bearer token.
    /// </summary>
    /// <param name="realm"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    private static UserClientRoleProtocolMapper CreateClientRoleMapper(Realm realm, Client client, Provider keycloakProvider) => new("client-role-mapper", new()
    {
        RealmId = realm.Id,
        ClientId = client.Id,
        ClientIdForRoleMappings = client.Name,
        ClaimName = "rolsito",
        Name = "role-mapper",
        ClaimValueType = "String",
        Multivalued = true
    }, new()
    {
        Provider = keycloakProvider
    });
}
