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

        private bool _debugMode = false;
        [Display(Name = "Debug Mode", Description = "Set to true for in-depth logging information")]
        [Category("Tebex Config")]
        public bool DebugMode
        {
            get => _debugMode;
            set => SetValue(ref _debugMode, value);
        }

        private string _secretKey = "Your Tebex Secret Key";
        [Display(Name = "Secret Key", Description = "Your Game Server key from https://creator.tebex.io/game-servers")]
        [Category("Tebex Config")]
        public string SecretKey { get => _secretKey; set => SetValue(ref _secretKey, value); }

        private bool _autoReportingEnabled = true;
        [Display(Name = "Auto Report Errors", Description = "Any errors will be automatically reported to Tebex")]
        [Category("Tebex Config")]
        public bool AutoReportingEnabled
        {
            get => _autoReportingEnabled;
            set => SetValue(ref _autoReportingEnabled, value);
        }

        private bool _disableRedeemCommand = false;
        [Display(Name = "Disable Redeem Command", Description = "Disable to prevent players from using /tebex:redeem")]
        [Category("Tebex Config")]
        public bool DisableRedeemCommand {get => _disableRedeemCommand; set => SetValue(ref _disableRedeemCommand, value);
        }
    }
}
