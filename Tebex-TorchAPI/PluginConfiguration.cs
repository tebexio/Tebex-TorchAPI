using System.ComponentModel;
using Torch;
using Torch.Views;
using VRage.Plugins;

namespace TebexSpaceEngineersPlugin
{
    public class PluginConfiguration : ViewModel
    {
        public PluginConfiguration()
        {
        }
        
        [Display(Name = "Debug Mode", Description = "Set to true for in-depth logging information")]
        [Category("Tebex Config")]
        public bool DebugMode = false;

        [Display(Name = "Secret Key", Description = "Your Game Server key from https://creator.tebex.io/game-servers")]
        [Category("Tebex Config")]
        public string SecretKey = "Your Tebex Secret Key";

        [Display(Name = "Auto Report Errors", Description = "Any errors will be automatically reported to Tebex")]
        [Category("Tebex Config")]
        public bool AutoReportingEnabled = true;
    }
}
