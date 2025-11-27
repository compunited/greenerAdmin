using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenerConfigurator.ClientCore.Utilities
{
    public class Constants
    {
        public const string BearerTokenKey = "bearer";



        //Authentication
        public static readonly string TenantName = "greenerswkunden";
        private static readonly string Tenant = $"{TenantName}.onmicrosoft.com";
        public static readonly string TenantId = "customer.greener.software";
        public static readonly string ClientId = "f9ca6647-c470-499d-912b-47b713d43cd1";
        public static readonly string SignInPolicy = "B2C_1_signup_signin";

        //public static readonly string[] Scopes = { TodoListScope };// new string[] { "offline_access" };
        public static readonly string[] Scopes = { $"https://{Tenant}/helloapi/demo.read" };// new string[] { "offline_access" };
        //public static readonly string[] Scopes = new string[] { "openid", "offline_access" };

        //public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/{TenantName}.onmicrosoft.com/{SignInPolicy}/oauth2/v2.0/token";
        public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
        public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";

    }
}
