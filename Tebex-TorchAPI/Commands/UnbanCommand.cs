using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class UnbanCommand : CommandModule
    {
        [Command("tebex.unban", "Players must be unbanned via your webstore.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexUnban()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            _adapter.ReplyPlayer(commandRunner, "You must unban players via your webstore.");
        }
    }
}