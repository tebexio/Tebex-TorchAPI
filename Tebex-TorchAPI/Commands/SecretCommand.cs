using System.Collections.Generic;
using Tebex.Adapters;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class SecretCommand : CommandModule
    {
        [Command("tebex.secret", "Sets your store's secret key.")]
        [Permission(MyPromoteLevel.Owner)]
        public void TebexSecret()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;

            if (args.Count != 1)
            {
                _adapter.ReplyPlayer(commandRunner, "Invalid syntax. Usage: \"tebex.secret <secret>\"");
                return;
            }

            _adapter.ReplyPlayer(commandRunner, "Setting your secret key...");
            BaseTebexAdapter.PluginConfig.SecretKey = args[0];
            _adapter.SaveConfiguration();

            // Reset store info so that we don't fetch from the cache
            BaseTebexAdapter.Cache.Instance.Remove("information");

            // Any failure to set secret key is logged to console automatically
            _adapter.FetchStoreInfo(info =>
            {
                _adapter.ReplyPlayer(commandRunner, "Successfully set your secret key.");
                _adapter.ReplyPlayer(commandRunner,
                    $"Store set as: {info.ServerInfo.Name} for the web store {info.AccountInfo.Name}");
            });
        }
    }
}