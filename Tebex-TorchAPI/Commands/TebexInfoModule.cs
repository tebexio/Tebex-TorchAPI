using System;
using NLog;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using Newtonsoft.Json.Linq;

namespace TebexTorchAPI.Commands
{
    public class TebexInfoModule : TebexCommandModule
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Command("tebex:info", "Return information about your webstore.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexInfo()
        {
            try
            {
                TebexApiClient wc = new TebexApiClient();
                wc.setPlugin(Tebex.Instance);
                wc.DoGet("information", this);
                wc.Dispose();
            }
            catch (TimeoutException)
            {
                Tebex.logWarning("Timeout!");
            }            
        }

        public override void HandleResponse(JObject response)
        {
            Tebex.Instance.information.id = (int)response["account"]["id"];
            Tebex.Instance.information.domain = (string)response["account"]["domain"];
            Tebex.Instance.information.gameType = (string)response["account"]["game_type"];
            Tebex.Instance.information.name = (string)response["account"]["name"];
            Tebex.Instance.information.currency = (string)response["account"]["currency"]["iso_4217"];
            Tebex.Instance.information.currencySymbol = (string)response["account"]["currency"]["symbol"];
            Tebex.Instance.information.serverId = (int)response["server"]["id"];
            Tebex.Instance.information.serverName = (string)response["server"]["name"];

            Tebex.logInfo("Server Information");
            Tebex.logInfo("=================");
            Tebex.logInfo("Server " + Tebex.Instance.information.serverName + " for webstore " + Tebex.Instance.information.name + "");
            Tebex.logInfo("Server prices are in " + Tebex.Instance.information.currency + "");
            Tebex.logInfo("Webstore domain: " + Tebex.Instance.information.domain + "");
        }

        public override void HandleError(Exception e)
        {
            Tebex.logError("We are unable to fetch your server details. Please check your secret key.");
        }

    }
}
