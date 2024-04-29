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
        public bool DisableRedeemCommand
        {
            get => _disableRedeemCommand;
            set => SetValue(ref _disableRedeemCommand, value);
        }

        private MyObjectBuilder_Toolbar _vanillaBacking;

        [XmlIgnore]
        private MyObjectBuilder_Toolbar VanillaDefaultToolbar => _vanillaBacking ?? (_vanillaBacking = new MyToolbar(MyToolbarType.Character, 9, 9).GetObjectBuilder());

        private MyObjectBuilder_Toolbar _defaultToolbar;
        
        public bool ShouldSerializeDefaultToolbar()
        {
            return _defaultToolbar != null;
        }

        /// <summary>
        /// Allows us to use Keen's serializer without losing previously stored config data
        /// </summary>
        public class ToolbarWrapper : IXmlSerializable
        {
            public MyObjectBuilder_Toolbar Data { get; set; }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                var ser = MyXmlSerializerManager.GetSerializer(typeof(MyObjectBuilder_Toolbar));
                var o = ser.Deserialize(reader);
                Data = (MyObjectBuilder_Toolbar)o;
            }

            public void WriteXml(XmlWriter writer)
            {
                var ser = MyXmlSerializerManager.GetSerializer(typeof(MyObjectBuilder_Toolbar));
                ser.Serialize(writer, Data);
            }

            public static implicit operator MyObjectBuilder_Toolbar(ToolbarWrapper o)
            {
                return o.Data;
            }

            public static implicit operator ToolbarWrapper(MyObjectBuilder_Toolbar o)
            {
                return new ToolbarWrapper(){Data = o};
            }
        }
    }
}
