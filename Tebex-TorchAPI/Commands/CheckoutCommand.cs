using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class CheckoutCommand : CommandModule
    {
        [Command("tebex:checkout", "Creates a link for a given package ID.")]
        [Permission(MyPromoteLevel.None)]
        public CheckoutCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            // Only argument will be the package ID of the item in question
            if (args.Count != 1)
            {
                _adapter.ReplyPlayer(commandRunner, "Invalid syntax: Usage \"tebex:checkout <packageId>\"");
                return;
            }

            // Lookup the package by provided input and respond with the checkout URL
            var package = _adapter.GetPackageByShortCodeOrId(args[0].Trim());
            if (package == null)
            {
                _adapter.ReplyPlayer(commandRunner, "A package with that ID was not found.");
                return;
            }

            _adapter.ReplyPlayer(commandRunner, "Creating your checkout URL...");
            _adapter.CreateCheckoutUrl(commandRunner.Identity.DisplayName, package, checkoutUrl =>
            {
                _adapter.ReplyPlayer(commandRunner, "Please visit the following URL to complete your purchase:");
                _adapter.ReplyPlayer(commandRunner, $"{checkoutUrl.Url}");
            }, error => { _adapter.ReplyPlayer(commandRunner, $"{error.ErrorMessage}"); });
        }
    }
}