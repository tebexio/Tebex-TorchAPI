using System;
using Tebex.Adapters;

namespace Tebex.API.Shared
{
    public static class TebexCommands
    {
        public static bool Handle(string fullCommand, string playerId, BaseTebexAdapter adapter)
        {
            
            if (!fullCommand.StartsWith("/tebex"))
            {
                return false;
            }

            char separator = fullCommand.Contains(":") ? ':' : ' '; // ex. "tebex:command" vs "tebex command"
            var tokens = fullCommand.Split(separator); // ["tebex", "command", "arg1"]
            var command = tokens[1];
            var args = tokens.CreateSubarray(2, tokens.Length - 2);
         
            TebexCommandContext commandContext = new TebexCommandContext(playerId, command, args, adapter);
            
            switch(command)
            {
                case "help":
                    return HelpCommand(commandContext);
                case "redeem":
                    return RedeemCommand(commandContext);
                case "forcecheck": 
                    return ForceCheckCommand(commandContext);
                case "secret": 
                    return SecretCommand(commandContext);
                case "debug": 
                    return DebugCommand(commandContext);
                default:
                    return false;
            }
        }
        
        public static bool HelpCommand(TebexCommandContext context)
        {
            context.ReplySender("Tebex Commands Available:");
            context.ReplySender( "tebex:help <secretKey>          - Sets your server's secret key.");
            context.ReplySender( "tebex:redeem                    - Checks for any pending purchases for your player.");
            context.ReplySender( "tebex:secret <secretKey>        - Sets your server's secret key.");
            context.ReplySender( "tebex:debug <true/false/on/off> - Enables or disables debug logging.");
            context.ReplySender("tebex:forcecheck                 - Forces the command queue to check for any pending purchases.");
            return true;
        }
            
        public static bool RedeemCommand(TebexCommandContext context)
        {
            return false;
        }
            
        public static bool SecretCommand(TebexCommandContext context)
        {
            return false;
        }

        public static bool DebugCommand(TebexCommandContext context)
        {
            return false;
        }
        
        public static bool ForceCheckCommand(TebexCommandContext context)
        {
            return false;
        }
        
        public class TebexCommandContext
        {
            public string SenderId { get; private set; }
            public string CommandName { get; private set; }
            public string[] Args { get; private set; }
            public BaseTebexAdapter Adapter { get; private set; }
            public object SenderRef { get; private set; }

            public TebexCommandContext(string senderId, string commandName, string[] args, BaseTebexAdapter adapter)
            {
                SenderId = senderId;
                CommandName = commandName;
                Args = args;
                Adapter = adapter;
                SenderRef = TryGetSender();
            }

            public object TryGetSender()
            {
                if (SenderRef != null)
                {
                    return SenderRef;
                }
                SenderRef = Adapter.GetPlayerRef(SenderId);
                return SenderRef;
            }

            public void ReplySender(string message)
            {
                Adapter.ReplyPlayer(SenderRef, message);
            }
        }
    }
}