using System.Collections.Generic;
using Tebex.Adapters;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class DebugCommand : CommandModule
    {
        [Command("tebex.debug", "Enables more in-depth logging.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexDebug()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;

            if (args.Count != 1)
            {
                _adapter.ReplyPlayer(commandRunner, "Usage: tebex.debug <on/off>");
                return;
            }

            if (args[0].Equals("on"))
            {
                BaseTebexAdapter.PluginConfig.DebugMode = true;
                _adapter.SaveConfiguration();
                _adapter.ReplyPlayer(commandRunner, "Debug mode is enabled.");
            }
            else if (args[0].Equals("off"))
            {
                BaseTebexAdapter.PluginConfig.DebugMode = false;
                _adapter.SaveConfiguration();
                _adapter.ReplyPlayer(commandRunner, "Debug mode is disabled.");
            }
            else
            {
                _adapter.ReplyPlayer(commandRunner, "Usage: tebex.debug <on/off>");
            }
        }
    }
}