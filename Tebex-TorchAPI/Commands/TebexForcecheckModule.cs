using System;
using NLog;
using Sandbox.Game.World;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using Newtonsoft.Json.Linq;

namespace TebexTorchAPI.Commands
{
    public class TebexForcecheckModule : TebexCommandModule
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        [Command("tebex:forcecheck", "Check for new commands.")]
        [Permission(MyPromoteLevel.Admin)]
        public void TebexForcecheck()
        {
            Tebex.logWarning("Checking for commands to be executed...");
            try
            {
                TebexApiClient wc = new TebexApiClient();
                wc.setPlugin(Tebex.Instance);
                wc.DoGet("queue", this);
                wc.Dispose();
            }
            catch (TimeoutException)
            {
                Tebex.logWarning("Timeout!");
            }

        }

        public override void HandleResponse(JObject response)
        {
            if ((int) response["meta"]["next_check"] > 0)
            {
                Tebex.Instance.nextCheck = (int) response["meta"]["next_check"];
                //Tebex.Instance.nextCheck = 60;
            }
            
            if ((bool) response["meta"]["execute_offline"])
            {
                try
                {
                    TebexCommandRunner.doOfflineCommands();
                }
                catch (Exception e)
                {
                    Tebex.logError(e.ToString());
                }
            }
            
            JArray players = (JArray) response["players"];

            var onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
            if (onlinePlayers.Count == 0)
            {
                return;
            }

            foreach (var player in players)
            {
                try
                {
                    ulong steamId = (ulong) player["uuid"];
                    long identityId = MySession.Static.Players.TryGetIdentityId(steamId);
                    MyIdentity targetPlayer = MySession.Static.Players.TryGetIdentity(identityId);

                    bool playerOnline = false;
                    foreach (var onlinePlr in onlinePlayers)
                    {
                        if (onlinePlr.Id.SteamId == steamId)
                        {
                            playerOnline = true;
                        }
                    }                    

                    if (playerOnline)
                    {
                        Tebex.logWarning("Execute commands for " + (string) targetPlayer.DisplayName + "(ID: "+ steamId.ToString()+")");
                        TebexCommandRunner.doOnlineCommands((int) player["id"], (string)targetPlayer.DisplayName,
                            steamId.ToString());


                    }
                }
                catch (Exception e)
                {
                    Tebex.logError(e.Message);
                }
            }
        }

        public override void HandleError(Exception e)
        {
            Tebex.logError("We are unable to fetch your server queue. Please check your secret key.");
            Tebex.logError(e.ToString());
        }         
    }
}