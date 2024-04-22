using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class PackagesCommand : CommandModule
    {
        [Command("tebex.packages", "Shows packages available for purchase.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexPackages()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            _adapter.GetPackages(packages => { TebexPlugin.PrintPackages(commandRunner, packages); });
        }
    }
}