using System.Collections.Generic;
using Tebex.Adapters;
using Tebex.API;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class RefreshCommand : CommandModule
    {
        [Command("tebex:refresh", "Refreshes store information, packages, and categories.")]
        [Permission(MyPromoteLevel.Admin)]
        public RefreshCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            _adapter.ReplyPlayer(commandRunner, "Refreshing listings...");
            BaseTebexAdapter.Cache.Instance.Remove("packages");
            BaseTebexAdapter.Cache.Instance.Remove("categories");

            _adapter.RefreshListings((code, body) =>
            {
                if (BaseTebexAdapter.Cache.Instance.HasValid("packages") &&
                    BaseTebexAdapter.Cache.Instance.HasValid("categories"))
                {
                    var packs = (List<TebexApi.Package>)BaseTebexAdapter.Cache.Instance.Get("packages").Value;
                    var categories = (List<TebexApi.Category>)BaseTebexAdapter.Cache.Instance.Get("categories").Value;
                    _adapter.ReplyPlayer(commandRunner,
                        $"Fetched {packs.Count} packages out of {categories.Count} categories");
                }
            });
        }
    }
}