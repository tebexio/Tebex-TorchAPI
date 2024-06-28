using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class ForceCheckCommand : CommandModule
    {
        [Command("tebex.forcecheck", "Force runs a check for packages to apply.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexForceCheck()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }
            
            _adapter.RefreshStoreInformation(true);
            _adapter.ProcessCommandQueue(true);
            _adapter.ProcessJoinQueue(true);
            _adapter.DeleteExecutedCommands(true);
        }
    }
}