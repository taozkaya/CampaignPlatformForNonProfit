using CampaignPlatformForNonProfit.CampaignPublisher;
using LinqToTwitter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitterClientDesktopTest
{
    class Program
    {
        static void Main(string[] args)
        {
       /*     string KeyVaultUrl = "https://aadpkeyvault.vault.azure.net/";
            string ConsumerKey = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterConsumerKey").Result;
            string TwitterConsumerSecret = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterConsumerSecret").Result;
            string TwitterAccessToken = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterAccessToken").Result;
            string TwitterAccessTokenSecret = Shared.GetTokenFromKeyVault(KeyVaultUrl, "TwitterAccessTokenSecret").Result;
            */
            // Media category - possible values are tweet_image, tweet_gif, tweet_video, and amplify_video
            // Type of media. e.g. image/jpg, image/png, or video/mp4.
            //     string mediaType = "image/jpg";
            //     string mediaCategory = "tweet_image";
            //    byte[] media = File.ReadAllBytes(@"C:\Users\taozkaya\Source\Repos\CampaignPlatformForNonProfit.CampaignPublisher\TwitterClientDesktopTest\be5.jpg");

            string mediaType = "jpg";
            string mediaCategory = "image";
            string message = "vid test";
            byte[] media = File.ReadAllBytes(@"C:\Users\taozkaya\Source\Repos\CampaignPlatformForNonProfit.CampaignPublisher\TwitterClientDesktopTest\be5.jpg");


       //     TwitterContext context = TwitterClient.GetTwitterContextAsync(ConsumerKey, TwitterConsumerSecret, TwitterAccessToken, TwitterAccessTokenSecret).Result;
        //    TwitterClient tc = new TwitterClient(context);
         //   string response = "";


            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload["mediaType"] = "jpg";
            payload["mediaCategory"] = "image";
            payload["message"] = "test";
            payload["media"] = media;
            string url = "https://contentpublisherapp.azurewebsites.net/api/CampaignPublisher?code=N0cMytWqQzb6RvJTQ/krWvtwgY6Gh3TLNJaUshlms4QBXY30fVBdvA==";

            string payloadString = JsonConvert.SerializeObject(payload, Formatting.Indented);

            HttpClient client = new HttpClient();
            HttpContent requestContent = new StringContent(payloadString, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = client.PostAsync(url, requestContent).Result;
            string response = httpResponse.Content.ReadAsStringAsync().Result;           
            /*
            if (mediaCategory.Contains("video"))
            {
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
            */
            Console.WriteLine(response);
                
            Console.ReadKey();
        }
    }
}


/*
         SendibleClient sendible = new SendibleClient(new HttpClient(), SendibleAPIKey, SendibleUsername, SendibleAppId);
   //      string token = sendible.GetAccessToken().Result;
   /*      string services = sendible.GetServices().Result;
         //      Console.WriteLine(token);
         byte[] media = File.ReadAllBytes(@"C:\Users\taozkaya\Source\Repos\CampaignPlatformForNonProfit.CampaignPublisher\TwitterClientDesktopTest\bethematch.jpg");
         string base64media = System.Convert.ToBase64String(media);
         */
