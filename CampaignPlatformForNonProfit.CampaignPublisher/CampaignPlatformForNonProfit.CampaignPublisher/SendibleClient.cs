using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CampaignPlatformForNonProfit.CampaignPublisher
{
    public class SendibleClient
    {
        HttpClient httpClient;
        String username;
        String APIKey;
        String AppId;

        public SendibleClient(HttpClient client, string APIKey, string username, string applicationId)
        {
            httpClient = client;
            this.APIKey = APIKey;
            this.username = username;
            AppId = applicationId;

            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("authorization", "Basic " + EncodeTo64(username+":"+ APIKey));
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public async Task<string> GetAccessToken()
        {
            try
            {
                    httpClient.BaseAddress = new Uri("https://api.sendible.com");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("", "")
            });
                    var result = await httpClient.GetAsync("/api/v2/auth?username=" + username + "&api_key=" + APIKey);
                    string resultContent = await result.Content.ReadAsStringAsync();
                //<html><body>You are being <a href="http://?access_token=ac67ccb59f4973eb093a482379941fe87ad13fa1&amp;username=bethecure2018">redirected</a>.</body></html>
                string accesstokenSubstring = resultContent.Substring(resultContent.IndexOf("access_token="));
                int startindex = accesstokenSubstring.IndexOf("=") + 1;
                int endIndex = accesstokenSubstring.IndexOf("&");

                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("authorization", "Basic " + APIKey);

                return accesstokenSubstring.Substring(startindex, endIndex - startindex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }

        public async Task<string> GetServices()
        {
            try
            {
                httpClient.BaseAddress = new Uri("https://api.sendible.com");
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("", "")
            });//api/v1/services.json?application_id=4b634537cc392cca820b61dab08&
                var result = await httpClient.GetAsync("/api/v1/services.json?application_id=" + AppId);
                string resultContent = await result.Content.ReadAsStringAsync();
                //<html><body>You are being <a href="http://?access_token=ac67ccb59f4973eb093a482379941fe87ad13fa1&amp;username=bethecure2018">redirected</a>.</body></html>
                return resultContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }

    }
}
