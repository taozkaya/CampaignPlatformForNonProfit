using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CampaignPlatformForNonProfit.CampaignPublisher;
using LinqToTwitter;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace CampaignPublisher
{
    public static class Function1
    {
        [FunctionName("CampaignPublisher")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            dynamic data = await req.Content.ReadAsAsync<object>();


            string KeyVaultUrl = "https://aadpkeyvault.vault.azure.net/";
            string ConsumerKey = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterConsumerKey").Result;
            string TwitterConsumerSecret = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterConsumerSecret").Result;
            string TwitterAccessToken = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterAccessToken").Result;
            string TwitterAccessTokenSecret = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterAccessTokenSecret").Result;

            // Media category - possible values are tweet_image, tweet_gif, tweet_video, and amplify_video
            // Type of media. e.g. image/jpg, image/png, or video/mp4.
            //     string mediaType = "jpg";
            //     string mediaCategory = "tweet_image";
            //    byte[] media = File.ReadAllBytes(@"C:\Users\taozkaya\Source\Repos\CampaignPlatformForNonProfit.CampaignPublisher\TwitterClientDesktopTest\be5.jpg");

            string mediaType = data.mediaType;
            string mediaCategory = data.mediaCategory;
            string message = data.message;
            byte[] media = data.media; 

            TwitterContext context = TwitterClient.GetTwitterContextAsync(ConsumerKey, TwitterConsumerSecret, TwitterAccessToken, TwitterAccessTokenSecret).Result;
            TwitterClient tc = new TwitterClient(context);
            string response = "";
            if (mediaCategory.Contains("video"))
            {
             //   if(media.Length <)

                mediaCategory = "tweet_video";
                mediaType = "video/" + mediaType;
                response = tc.TweetAsync(message, media, mediaType, mediaCategory).Result;
            }
            else
            {
                mediaCategory = "tweet_image";
                mediaType = "image/" + mediaType;
                response = tc.TweetImageAsync(message, media, mediaType, mediaCategory).Result;
            }
            log.Info(response);

            return req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
