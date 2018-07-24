using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignPlatformForNonProfit.CampaignPublisher
{
    public class TwitterClient
    {
        TwitterContext twitterContext;
        public static async Task<TwitterContext> GetTwitterContextAsync(string ConsumerKey, string TwitterConsumerSecret, string TwitterAccessToken, string TwitterAccessTokenSecret)
        {
            IAuthorizer auth = GetAuthorizer(ConsumerKey, TwitterConsumerSecret, TwitterAccessToken, TwitterAccessTokenSecret);

            await auth.AuthorizeAsync();
            return new TwitterContext(auth);
        }

        public TwitterClient(TwitterContext context)
        {
            twitterContext = context;
        }

        public async Task<string> TweetImageAsync(string message, byte[] mediaByteArray, string extension, string mediaType)
        {
            var imageUploadTasks =
                new List<Task<Media>>
                {
                    twitterContext.UploadMediaAsync(mediaByteArray, extension, mediaType),
                };
            await Task.WhenAll(imageUploadTasks);
            List<ulong> mediaIds =
                (from tsk in imageUploadTasks
                 select tsk.Result.MediaID)
                .ToList();
            Status tweet = await twitterContext.TweetAsync(message, mediaIds);
            return tweet.Text;
        }

        public async Task<string> TweetAsync(string message, byte[] mediaByteArray, string extension, string mediaType)
        {
            string response = "";
            string status = message;

             Media media = await twitterContext.UploadMediaAsync(mediaByteArray, extension, mediaType);

            Media mediaStatusResponse = null;
            
            do
            {
                if (mediaStatusResponse != null)
                {
                    int checkAfterSeconds = mediaStatusResponse?.ProcessingInfo?.CheckAfterSeconds ?? 0;
                    Console.WriteLine($"Twitter video testing in progress - waiting {checkAfterSeconds} seconds.");
                    await Task.Delay(checkAfterSeconds * 1000);
                }
                
                mediaStatusResponse =
                    await
                    (from stat in twitterContext.Media
                     where stat.MediaID == media.MediaID && stat.Type == MediaType.Status
                     select stat) // stat.Type == MediaType.Status &&
                    //       stat.MediaID == media.MediaID
                    // select stat)
                    .SingleOrDefaultAsync();
            } while (mediaStatusResponse?.ProcessingInfo?.State == MediaProcessingInfo.InProgress);
            
            if (mediaStatusResponse?.ProcessingInfo?.State == MediaProcessingInfo.Succeeded)
            {
                Status tweet = await twitterContext.TweetAsync(status, new ulong[] { media.MediaID });

                if (tweet != null)
                    response = $"Tweet sent: {tweet.Text}";
            }
            else
            {
                MediaError error = mediaStatusResponse?.ProcessingInfo?.Error;

                if (error != null)
                    response = $"Request failed - Code: {error.Code}, Name: {error.Name}, Message: {error.Message}";
            }
            return response;
        }

        private static IAuthorizer GetAuthorizer(string ConsumerKey, string TwitterConsumerSecret, string TwitterAccessToken, string TwitterAccessTokenSecret)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ConsumerKey,
                    ConsumerSecret = TwitterConsumerSecret,
                    AccessToken = TwitterAccessToken,
                    AccessTokenSecret = TwitterAccessTokenSecret
                }
            };
            return auth;
        }


    }

}
