using System.Collections.Generic;
using System.Net;
using Tebex.Triage;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class ReportCommand : CommandModule
    {
        [Command("tebex.report", "Reports an issue to the Tebex development team.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexReport()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            
            if (args.Count == 0) // require /confirm to send
            {
                _adapter.ReplyPlayer(commandRunner,
                    "Please run `tebex.report confirm 'Your description here'` to submit your report. The following information will be sent to Tebex: ");
                _adapter.ReplyPlayer(commandRunner, "- Your game version, store id, and server IP.");
                _adapter.ReplyPlayer(commandRunner, "- Your username and IP address.");
                _adapter.ReplyPlayer(commandRunner,
                    "- Please include a short description of the issue you were facing.");
            }

            if (args.Count == 2 && args[0] == "confirm")
            {
                _adapter.ReplyPlayer(commandRunner, "Sending your report to Tebex...");

                var triageEvent = new TebexTriage.ReportedTriageEvent();
                triageEvent.GameId = "Unturned";
                triageEvent.FrameworkId = "RocketMod";
                triageEvent.PluginVersion = TebexPlugin.GetPluginVersion();
                triageEvent.ServerIp = "0.0.0.0"; //FIXME
                triageEvent.ErrorMessage = "Player Report: " + args[1];
                triageEvent.Trace = "";
                triageEvent.Metadata = new Dictionary<string, string>();
                triageEvent.Username = commandRunner.DisplayName + "/" + commandRunner.IdentityId;
                triageEvent.UserIp = "0.0.0.0"; //FIXME

                _adapter.ReportManualTriageEvent(triageEvent,
                    (code, body) => { _adapter.ReplyPlayer(commandRunner, "Your report has been sent. Thank you!"); },
                    (code, body) =>
                    {
                        _adapter.ReplyPlayer(commandRunner,
                            "An error occurred while submitting your report. Please contact our support team directly.");
                        _adapter.ReplyPlayer(commandRunner, "Error: " + body);
                    });

                return;
            }

            _adapter.ReplyPlayer(commandRunner, "Usage: tebex.report <confirm> '<message>'");
        }
    }
}