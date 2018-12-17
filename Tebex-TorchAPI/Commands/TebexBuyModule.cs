using System;
using NLog;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRage.Game;
using Newtonsoft.Json.Linq;
using Sandbox.Game;

namespace TebexTorchAPI.Commands
{
    public class TebexBuyModule : CommandModule
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Command("donate", "Return the URL for your webstore.")]
        [Permission(MyPromoteLevel.None)]
        public void TebexDonate()
        {
            if (Context.Player != null)
            {
                MyVisualScriptLogicProvider.OpenSteamOverlay("https://steamcommunity.com/linkfilter/?url=" + System.Uri.EscapeUriString(Tebex.Instance.information.domain), Context.Player.IdentityId);
            }
            Context.Respond("To donate, please visit our webstore: " + Tebex.Instance.information.domain);
        }
    }
}
