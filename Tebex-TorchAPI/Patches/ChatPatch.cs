
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Tebex.API.Shared;

namespace TebexSpaceEngineersPlugin.Patches {
    //Marks the class for further inspection by the controller
    [PatchController.PatchingClass]
    public class ChatPatch {

        public ChatPatch() {
            //Constructor
        }



        //return true if you want "RaiseChatMessageRecieved to continue (so server continues processing and distributing to connected clients)"
        //return false to terminate process - Usually in the case after you handled the command.

        //Okay - this is a patch in which i want to be prefixed to the TargetMethod
        [PatchController.PrefixMethod]
        [PatchController.TargetMethod(Type = typeof(MyMultiplayerBase), Method = "RaiseChatMessageReceived")]
        public static bool ProcessChat(ulong steamUserID, string messageText, ChatChannel channel, long targetId, string customAuthorName = null) {
            //Filter out any chat that is not from a valid steam user | | Filter out if not going to  either of these channels (Global chat)
            if (!steamUserID.ToString().StartsWith("7") || (channel != ChatChannel.Global && channel != ChatChannel.GlobalScripted))
                return true;

            //Ensure chat starts with command prefix - you could define this in the config or make it whatever you want
            //Be weary of mod conficts since keen have no standard system for registering/handling commands.
            if (!messageText.StartsWith("/tebex"))
            {
                var success = TebexCommands.Handle(messageText, steamUserID.ToString(), TebexPlugin.GetAdapter());
                var shouldContinue = !success;
                return shouldContinue;
            }

            //Do command processing.
            VRage.Utils.MyLog.Default.WriteLineAndConsole("Command recieved and processing");
            return false;
        }
    }
}
