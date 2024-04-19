using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class HelpCommand : CommandModule
    {
        [Command("tebex:help", "Prints all available commands for your promotion level.")]
        [Permission(MyPromoteLevel.Admin)]
        public HelpCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var player = Context.Player;
            var args = Context.Args;
            
            _adapter.ReplyPlayer(player, "Tebex Commands Available:");
            if (player.PromoteLevel >= MyPromoteLevel.Admin)
            {
                _adapter.ReplyPlayer(player, "-- Administrator Commands --");
                _adapter.ReplyPlayer(player, "tebex:secret <secretKey>          - Sets your server's secret key.");
                _adapter.ReplyPlayer(player, "tebex:debug <on/off>              - Enables or disables debug logging.");
                _adapter.ReplyPlayer(player,
                    "tebex:sendlink <player> <packId>  - Sends a purchase link to the provided player.");
                _adapter.ReplyPlayer(player,
                    "tebex:forcecheck                  - Forces the command queue to check for any pending purchases.");
                _adapter.ReplyPlayer(player,
                    "tebex:refresh                     - Refreshes store information, packages, categories, etc.");
                _adapter.ReplyPlayer(player,
                    "tebex:report                      - Generates a report for the Tebex support team.");
                _adapter.ReplyPlayer(player,
                    "tebex:ban <playerId>              - Bans a player from using your Tebex store.");
                _adapter.ReplyPlayer(player,
                    "tebex:lookup <playerId>           - Looks up store statistics for the given player.");
            }

            _adapter.ReplyPlayer(player, "-- User Commands --");
            _adapter.ReplyPlayer(player,
                "tebex:info                       - Get information about this server's store.");
            _adapter.ReplyPlayer(player,
                "tebex:categories                 - Shows all item categories available on the store.");
            _adapter.ReplyPlayer(player,
                "tebex:packages <opt:categoryId>  - Shows all item packages available in the store or provided category.");
            _adapter.ReplyPlayer(player,
                "tebex:checkout <packId>          - Creates a checkout link for an item. Visit to purchase.");
            _adapter.ReplyPlayer(player,
                "tebex:buy                        - Opens this server's webstore.");
        }
    }
}