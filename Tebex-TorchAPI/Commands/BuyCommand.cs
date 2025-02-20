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
        [Command("buy", "Links a user to your webstore.")]
        [Permission(MyPromoteLevel.None)]
        public void TebexBuy()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

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
            else
            {
                _adapter.LogError("Cannot run buy command for player of type: " + commandRunner.GetType().FullName);
            }
        }
    }
}