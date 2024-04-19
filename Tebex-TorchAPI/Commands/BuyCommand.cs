using Tebex.Adapters;
using Tebex.API;
using TebexSpaceEngineersPlugin;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class BuyCommand : CommandModule
    {
        [Command("tebex:buy", "Links a user to your webstore.")]
        [Permission(MyPromoteLevel.None)]
        public BuyCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            //FIXME
            /*
            if (commandRunner is ConsolePlayer)
            {
                _adapter.ReplyPlayer(commandRunner,
                    "/tebex:buy cannot be executed via console. Use tebex:sendlink <username> <packageId> to specify a target player.");
                return;
            }*/

            if (commandRunner is IMyPlayer)
            {
                var player = commandRunner;
                _adapter.LogInfo($"Buy command received from 'steam:{commandRunner.IdentityId}/ign:{player.DisplayName}'");
                if (BaseTebexAdapter.Cache.Instance.HasValid("information"))
                {
                    var storeInfo = (TebexApi.TebexStoreInfo)BaseTebexAdapter.Cache.Instance.Get("information").Value;
                    _adapter.ReplyPlayer(player, "To buy packages from our webstore, please visit: " + storeInfo.AccountInfo.Domain);
                }
                else
                {
                    _adapter.LogError("Store information is not available. Check secret key and run /tebex:refresh");
                    _adapter.ReplyPlayer(player, "This store is not yet setup.");
                }
            }
        }
    }
}