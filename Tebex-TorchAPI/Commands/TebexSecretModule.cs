using System;
using Newtonsoft.Json.Linq;
using NLog;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexTorchAPI.Commands
{
    public class TebexSecretModule : TebexCommandModule
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Command("tebex:secret", "Set your secret key.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexSecret(string secret)
        {
            
            Tebex.Instance.Config.Secret = secret;
            
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
            
            Tebex.Instance.information.id = (int) response["account"]["id"];
            Tebex.Instance.information.domain = (string) response["account"]["domain"];
            Tebex.Instance.information.gameType = (string) response["account"]["game_type"];
            Tebex.Instance.information.name = (string) response["account"]["name"];
            Tebex.Instance.information.currency = (string) response["account"]["currency"]["iso_4217"];
            Tebex.Instance.information.currencySymbol = (string) response["account"]["currency"]["symbol"];
            Tebex.Instance.information.serverId = (int) response["server"]["id"];
            Tebex.Instance.information.serverName = (string) response["server"]["name"];
            
            Tebex.logWarning("Your secret key has been validated! Webstore Name: " + Tebex.Instance.information.name);
            Context.Respond("Your secret key has been validated! Webstore Name: " + Tebex.Instance.information.name);
        }

        public override void HandleError(Exception e)
        {
            Tebex.logError("We were unable to validate your secret key.");
            Context.Respond("We were unable to validate your secret key.");
        }
    }
}