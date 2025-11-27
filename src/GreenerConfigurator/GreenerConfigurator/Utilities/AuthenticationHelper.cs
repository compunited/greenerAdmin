using GreenerConfigurator.ClientCore.Models;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using GreenerConfigurator.ClientCore.Utilities;

namespace GreenerConfigurator.Utilities
{
    public static class AuthenticationHelper
    {

        #region [ Public Property(s) ]

        public static UserModel CurerntUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
            }
        }

        #endregion

        #region [ Public Method(s) ]

        public static async Task<string> GetToken()
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(token) ||
                    DateTime.Now.Subtract(lastRefreshDataTime).TotalMinutes > 1430)
                if (true)
                {
                    var resultLoing = await Login();
                }

            result = token;

            return result;
        }

        public static void Init(Window window)
        {

            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                                        //.WithAuthority(Constants.Authority)
                                        .WithB2CAuthority(Constants.AuthoritySignIn)
                                        .WithDesktopFeatures()
                                         .WithRedirectUri($"msal{Constants.ClientId}://auth")
                                         //.WithRedirectUri("http://localhost")
                                         .Build();

            TokenCacheHelper.Bind(AuthenticationClient.UserTokenCache);

            currentWindow = window;
        }

        public static async Task<bool> Logout()
        {

            bool result = false;

            IEnumerable<IAccount> accounts = null;

            try
            {
                // Look for existing account
                accounts = await AuthenticationClient.GetAccountsAsync();

                if (accounts.Count() >= 1)
                {
                    await AuthenticationClient.RemoveAsync(accounts.First());
                    result = true;
                }
            }
            catch
            {
                if (accounts != null && accounts.Count() > 0)
                    await AuthenticationClient.RemoveAsync(accounts.FirstOrDefault());
                // Do nothing - the user isn't logged in
                result = true;
            }
            finally
            {
                token = string.Empty;
                //  SecureStorage.Remove(Constants.BearerTokenKey);
                // App.Current.MainPage = new LoginPage();
            }

            return result;
        }

        #endregion

        #region [ Private Field(s) ]

        private static string token = string.Empty;

        private static DateTime lastRefreshDataTime = DateTime.Now;

        private static Window currentWindow = null;

        private static UserModel _currentUser = null;

        #endregion

        #region [ Private Property(s) ]

        private static IPublicClientApplication AuthenticationClient { get; set; }

        #endregion

        #region [ Private Method(s) ]

        private static async Task<bool> Refreshtoken()
        {
            bool result = false;

            var accounts = await AuthenticationClient.GetAccountsAsync();

            if (accounts.Count() >= 1)
            {
                AuthenticationResult authResult = await AuthenticationClient
                                                        .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                                                        .ExecuteAsync();

                lastRefreshDataTime = DateTime.Now;
                token = authResult.IdToken;
                FetchUserInfoFromToken(authResult);

                // await SecureStorage.SetAsync(Constants.BearerTokenKey, authResult.IdToken);
                result = true;
            }

            return result;
        }

        private static async Task<bool> Login()
        {
            bool result = false;

            try
            {
                //ResultText.Text = "";
                var authResult = await AuthenticationClient.AcquireTokenInteractive(Constants.Scopes)
                    .WithParentActivityOrWindow(new WindowInteropHelper(currentWindow).Handle)
                    .ExecuteAsync();

                token = authResult.IdToken;
                lastRefreshDataTime = DateTime.Now;
                FetchUserInfoFromToken(authResult);
            }
            catch (MsalException ex)
            {
                #region OLD
                //try
                //{
                //    if (ex.Message.Contains("AADB2C90118"))
                //    {
                //        var authResult = await App.AuthenticationClient.AcquireTokenInteractive(Constants.Scopes)
                //            .WithParentActivityOrWindow(new WindowInteropHelper(window).Handle)
                //            .WithPrompt(Prompt.SelectAccount)
                //            //.WithB2CAuthority(Constants.AuthorityResetPassword)
                //            .ExecuteAsync();
                //        Debug.WriteLine(authResult.IdToken);
                //    }
                //    else
                //    {
                //        //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{ex}";
                //    }
                //}
                //catch (Exception exe)
                //{
                //    //ResultText.Text = $"Error Acquiring Token:{Environment.NewLine}{exe}";
                //} 
                #endregion
                LogHelper.LogError(ex.ToString());
            }
            catch( Exception exp)
            {
                LogHelper.LogError(exp.ToString());
            }

            return result;
        }

        private static void FetchUserInfoFromToken(AuthenticationResult authResult)
        {
            if (authResult != null)
            {
                JObject user = ParseIdToken(authResult.IdToken);
                _currentUser = new UserModel();


                _currentUser.Name = user["name"]?.ToString();

                if (user["emails"] is JArray emails)
                {
                    _currentUser.Email = emails[0].ToString();
                }

                _currentUser.ISS += user["iss"]?.ToString();
            }
        }

        private static JObject ParseIdToken(string idToken)
        {
            // Parse the idToken to get user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        private static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        #endregion

    }
}
