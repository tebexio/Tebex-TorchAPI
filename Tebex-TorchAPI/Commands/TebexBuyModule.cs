using System;
using NLog;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using Newtonsoft.Json.Linq;

namespace TebexTorchAPI.Commands
{
    public class TebexBuyModule : CommandModule
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Command("donate", "Return the URL for your webstore.")]
        [Permission(MyPromoteLevel.None)]
        public void TebexDonate()
        {
            Context.Respond("To donate, please visit our webstore at " + Tebex.Instance.information.domain);
        }

    }
}
