using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class InfoCommand : CommandModule
    {
        [Command("tebex.info", "Shows information about the connected webstore.")]
        [Permission(MyPromoteLevel.None)]
        public void TebexInfo()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var player = Context.Player;
            var args = Context.Args;
            
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(player, "Tebex is not setup.");
                return;
            }

            _adapter.ReplyPlayer(player, "Getting store information...");
            _adapter.FetchStoreInfo(info =>
            {
                _adapter.ReplyPlayer(player, "Information for this server:");
                _adapter.ReplyPlayer(player, $"{info.ServerInfo.Name} for webstore {info.AccountInfo.Name}");
                _adapter.ReplyPlayer(player, $"Server prices are in {info.AccountInfo.Currency.Iso4217}");
                _adapter.ReplyPlayer(player, $"Webstore domain {info.AccountInfo.Domain}");
            });
        }
    }
}