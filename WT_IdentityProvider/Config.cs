using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Marvin.IDP
{
    public static class Config
    {
        // test users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "2",
                    Username = "riste",
                    Password = "123456",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Riste"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "Main Road 1"),
                        new Claim("role", "Admin"),
                        new Claim("subscriptionlevel", "FreeUser"),
                        new Claim("country", "nl")
                    }
                },
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Frank",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Frank"),
                        new Claim("family_name", "Underwood"),
                        new Claim("address", "Big Street 2"),
                        new Claim("role", "PayingUser"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "be")
                    }
                }
            };
        }

        // identity-related resources (scopes)
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                     new List<string>() { "role" }),
                new IdentityResource(
                    "country",
                    "The country you're living in",
                    new List<string>() { "country" }),
                new IdentityResource(
                    "subscriptionlevel",
                    "Your subscription level",
                    new List<string>() { "subscriptionlevel" })
            };
        }

        // api-related resources (scopes)
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("wtapi", "WT_API_APP Gallery API", new List<string>() {"role" } )
                {
                     ApiSecrets = { new Secret("apisecret".Sha256()) } //used for token retrospection endpoint if used Reference Tokens instead JWT 
                }

            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "WT_MVC_App",
                    ClientId = "wtmvcapp",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    //AccessTokenType = AccessTokenType.Reference, //(not self-contained like JWT...with JWT api doesnt communicate with IDP..with reference it has to go to the IDP..)
                    //IdentityTokenLifetime = ... default is 5 min
                    //AuthorizationCodeLifetime = ... default is 5 min
                    AccessTokenLifetime = 180, //sets access token to 3 min...after this the api will access deny the request
                    AllowOfflineAccess = true, //for refresh tokens...
                    //AbsoluteRefreshTokenLifetime = ... default is 30 days
                    UpdateAccessTokenClaimsOnRefresh = true, //if the claims are changed when refreshing the access token to be taken....
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44381/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44381/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "wtapi",
                        "country",
                        "subscriptionlevel"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
             };

        }
    }
}
