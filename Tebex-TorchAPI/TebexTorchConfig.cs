using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sandbox.Game.Screens.Helpers;
using TebexSpaceEngineersPlugin.Commands;
using Torch;
using Torch.Views;
using VRage;
using VRage.Game;
using VRage.Plugins;

namespace TebexSpaceEngineersPlugin
{
    public class TebexTorchConfig : ViewModel
    {
        public TebexTorchConfig()
        {
        }

        private bool _debugMode = false;
        [Display(Name = "Debug Mode", Description = "Set to true for in-depth logging information")]
        public bool DebugMode
        {
            get => _debugMode;
            set => SetValue(ref _debugMode, value);
        }

        private string _secretKey = "Your Tebex Secret Key";
        [Display(Name = "Secret Key", Description = "Your Game Server key from https://creator.tebex.io/game-servers")]
        public string SecretKey { get => _secretKey; set => SetValue(ref _secretKey, value); }

        private bool _autoReportingEnabled = true;
        [Display(Name = "Auto Report Errors", Description = "Any errors will be automatically reported to Tebex")]
        public bool AutoReportingEnabled
        {
            get => _autoReportingEnabled;
            set => SetValue(ref _autoReportingEnabled, value);
        }
    }
}
