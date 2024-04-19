using System.Collections.Generic;
using Torch.Commands;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class SendLinkCommand : CommandModule
    {
        public SendLinkCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            if (args.Count != 2)
            {
                _adapter.ReplyPlayer(commandRunner, "Usage: tebex.sendlink <username> <packageId>");
                return;
            }

            var username = args[0].Trim();
            var package = _adapter.GetPackageByShortCodeOrId(args[1].Trim());
            if (package == null)
            {
                _adapter.ReplyPlayer(commandRunner, "A package with that ID was not found.");
                return;
            }

            _adapter.ReplyPlayer(commandRunner,
                $"Creating checkout URL with package '{package.Name}'|{package.Id} for player {username}");
            var player = _adapter.GetPlayerRef(username) as IMyPlayer;
            if (player == null)
            {
                _adapter.ReplyPlayer(commandRunner, "Couldn't find that player on the server.");
                return;
            }

            _adapter.CreateCheckoutUrl(player.Identity.DisplayName, package, checkoutUrl =>
            {
                _adapter.ReplyPlayer(player, "Please visit the following URL to complete your purchase:");
                _adapter.ReplyPlayer(player, $"{checkoutUrl.Url}");
            }, error => { _adapter.ReplyPlayer(player, $"{error.ErrorMessage}"); });
        }
    }
}