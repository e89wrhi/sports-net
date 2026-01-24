using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Identity.Identity.Constants;
using IdentityModel;

namespace Identity.Configurations;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new(Constants.StandardScopes.MatchApi),
            new(Constants.StandardScopes.EventApi),
            new(Constants.StandardScopes.VoteApi),
            new(Constants.StandardScopes.IdentityApi),
            new(Constants.StandardScopes.SportModularMonolith),
            new(JwtClaimTypes.Role, new List<string> {"role"})
        };


    public static IList<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new(Constants.StandardScopes.MatchApi)
            {
                Scopes = { Constants.StandardScopes.MatchApi }
            },
            new(Constants.StandardScopes.EventApi)
            {
                Scopes = { Constants.StandardScopes.EventApi }
            },
            new(Constants.StandardScopes.VoteApi)
            {
                Scopes = { Constants.StandardScopes.VoteApi }
            },
            new(Constants.StandardScopes.IdentityApi)
            {
                Scopes = { Constants.StandardScopes.IdentityApi }
            },
            new(Constants.StandardScopes.SportModularMonolith)
            {
                Scopes = { Constants.StandardScopes.SportModularMonolith }
            },
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    JwtClaimTypes.Role, // Include roles scope
                    Constants.StandardScopes.EventApi,
                    Constants.StandardScopes.MatchApi,
                    Constants.StandardScopes.VoteApi,
                    Constants.StandardScopes.IdentityApi,
                    Constants.StandardScopes.SportModularMonolith,
                },
                AccessTokenLifetime = 3600,  // authorize the client to access protected resources
                IdentityTokenLifetime = 3600, // authenticate the user,
                AlwaysIncludeUserClaimsInIdToken = true // Include claims in ID token
            }
        };
}