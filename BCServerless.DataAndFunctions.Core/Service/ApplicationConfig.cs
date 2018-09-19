using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IApplicationConfig
    {
        string Database { get; }
        string Collection { get; }
        string Endpoint { get; }
        string AuthKey { get; }
        string B2CTenant { get; }
        string B2CClient { get; }
        string B2CSecret { get; }
        bool AssignNewUserAdminRole { get; }
    }

    public class ApplicationConfig : IApplicationConfig
    {
        private string _database;
        private string _collection;
        private string _endpoint;
        private string _authKey;
        private string _b2CTenant;
        private string _b2CClient;
        private string _b2CSecret;
        private bool _assignNewUserAdminRole;
        private static KeyVaultClient _kvClient;
        private static HttpClient _client;
        private static AzureServiceTokenProvider _azureServiceTokenProvider;

        
        private static string GetValueFromVault(string keyName)
        {
            try
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                }

                if (_azureServiceTokenProvider == null)
                {
                    _azureServiceTokenProvider = new AzureServiceTokenProvider();
                }

                if (_kvClient == null)
                {
                    _kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(_azureServiceTokenProvider.KeyVaultTokenCallback), _client);
                }
                string vaultUrl = ConfigurationManager.AppSettings["vaultUrl"];
                var secretResponse = _kvClient.GetSecretAsync(vaultUrl, keyName).Result;
                string valueFromVault = secretResponse.Value;
                return valueFromVault;
            }
            catch(Exception exp)
            {
                throw new Exception("Unable to retrieve value from key vault for " + keyName, exp);
            }
        }


        public string Database => ConfigurationManager.AppSettings["database"];
        public string Collection => ConfigurationManager.AppSettings["collectionId"];
        public string Endpoint => ConfigurationManager.AppSettings["Endpoint"];
        public string AuthKey => ConfigurationManager.AppSettings["AuthKey"];
        public string B2CTenant => ConfigurationManager.AppSettings["B2CTenant"];
        //public string B2CClient => ConfigurationManager.AppSettings["B2CClient"];
        public string B2CClient => GetValueFromVault("B2CClient");
        public string B2CSecret => GetValueFromVault("B2CSecret");
        public bool AssignNewUserAdminRole => TryParse(ConfigurationManager.AppSettings["AssignNewUserAdminRole"]);

        private bool TryParse(string value)
        {
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }
    }
}
