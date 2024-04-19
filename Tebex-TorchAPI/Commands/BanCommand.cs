using System.Linq;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class BanCommand : CommandModule
    {
        [Command("tebex:ban", "Bans a user from using your webstore. They must be unbanned from your webstore panel.")]
        [Permission(MyPromoteLevel.Admin)]
        public BanCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            var reason = "";
            if (args.Count == 0)
            {
                _adapter.ReplyPlayer(commandRunner, "Usage: tebex.ban <playerName> <optional:reason>");
                return;
            }

            if (args.Count == 2) reason = args[1];
            var foundTargetPlayer = _adapter.GetPlayerRef(args[0].Trim()) as IMyPlayer;
            if (foundTargetPlayer == null)
            {
                _adapter.ReplyPlayer(commandRunner, "Could not find that player on the server.");
                return;
            }

            reason = string.Join(" ", args.Skip(1));
            _adapter.ReplyPlayer(commandRunner,
                $"Processing ban for player {foundTargetPlayer.Identity.DisplayName} with reason '{reason}'");
            _adapter.BanPlayer(foundTargetPlayer.Identity.DisplayName, TebexPlugin.GetPlayerIp((ulong)foundTargetPlayer.Identity.IdentityId), reason,
                (code, body) => { _adapter.ReplyPlayer(commandRunner, "Player banned successfully."); },
                error => { _adapter.ReplyPlayer(commandRunner, $"Could not ban player. {error.ErrorMessage}"); });
        }
    }
}