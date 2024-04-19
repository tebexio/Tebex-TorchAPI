using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Tebex.API;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    [Command("tebex:lookup", "Looks up user statistics from your webstore.")]
    [Permission(MyPromoteLevel.Admin)]
    public class LookupCommand : CommandModule
    {
        public LookupCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            if (args.Count != 1)
            {
                _adapter.ReplyPlayer(commandRunner, "Usage: tebex.lookup <playerId/playerUsername>");
                return;
            }

            // Try to find the given player
            var target = _adapter.GetPlayerRef(args[0]) as IMyPlayer;
            if (target == null)
            {
                _adapter.ReplyPlayer(commandRunner, $"Could not find a player matching the name or id {args[0]}.");
                return;
            }

            _adapter.GetUser(target.Identity.IdentityId.ToString(), (code, body) =>
            {
                var response = JsonConvert.DeserializeObject<TebexApi.UserInfoResponse>(body);
                _adapter.ReplyPlayer(commandRunner, $"Username: {response.Player.Username}");
                _adapter.ReplyPlayer(commandRunner, $"Id: {response.Player.Id}");
                _adapter.ReplyPlayer(commandRunner,
                    $"Payments Total: ${response.Payments.Sum(payment => payment.Price)}");
                _adapter.ReplyPlayer(commandRunner, $"Chargeback Rate: {response.ChargebackRate}%");
                _adapter.ReplyPlayer(commandRunner, $"Bans Total: {response.BanCount}");
                _adapter.ReplyPlayer(commandRunner, $"Payments: {response.Payments.Count}");
            }, error => { _adapter.ReplyPlayer(commandRunner, error.ErrorMessage); });
        }
    }
}