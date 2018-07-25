using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignPlatformForNonProfit.CampaignPublisher
{
    public class Shared
    {
        public static async Task<string> GetTokenFromKeyVault(string url, string tokenName)
        {
            AzureServiceTokenProvider azureTokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureTokenProvider.KeyVaultTokenCallback));
            SecretBundle secret = await keyVaultClient.GetSecretAsync(url, tokenName);
            string client_secret = secret.Value;
            return client_secret;
        }
    }
}
