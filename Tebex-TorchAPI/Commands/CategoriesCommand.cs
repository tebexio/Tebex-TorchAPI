using System.Collections.Generic;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;

namespace TebexSpaceEngineersPlugin.Commands
{
    public class CategoriesCommand : CommandModule
    {
        [Command("tebex:categories", "Lists package categories from your webstore.")]
        [Permission(MyPromoteLevel.None)]
        public CategoriesCommand()
        {
            var _adapter = TebexPlugin.GetAdapter();
            var commandRunner = Context.Player;
            var args = Context.Args;
            
            if (!_adapter.IsReady)
            {
                _adapter.ReplyPlayer(commandRunner, "Tebex is not setup.");
                return;
            }

            _adapter.GetCategories(categories =>
            {
                TebexPlugin.PrintCategories(commandRunner, categories);
            });
        }
    }
}