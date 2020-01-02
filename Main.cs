using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Wox.Plugin.Zeldathon
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;

            var currentGame = GetCurrentGame();
            var donationTotal = GetDonationTotal();

            return new List<Result>
            {
                new Result
                {
                    IcoPath = "Images\\zeldathon.png",
                    SubTitle = $"{currentGame}",
                    Title = "Current game",
                    Action = _ => true
                },
                new Result
                {
                    IcoPath = "Images\\zeldathon.png",
                    SubTitle = $"{donationTotal}",
                    Title = "Donation total",
                    Action = _ => true
                }
            };
        }

        private static string GetDonationTotal()
        {
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString("https://zeldathon.net/api/Kinstone/total");
                var responseArray = JArray.Parse(response);
                return (string) responseArray[0];
            }
        }

        private static string GetCurrentGame()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("client-id", "jzkbprff40iqj646a697cyrvl0zt2m6");

                var response = webClient.DownloadString("https://api.twitch.tv/kraken/channels/supermcgamer");
                var jsonObject = JObject.Parse(response);

                return (string) jsonObject["game"];
            }
        }
    }
}
