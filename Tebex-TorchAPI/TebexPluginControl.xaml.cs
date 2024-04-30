using System.Windows;
using System.Windows.Controls;

namespace TebexSpaceEngineersPlugin
{
    public partial class TebexPluginControl : UserControl
    {
        private TebexPlugin Plugin { get;  }
        
        public TebexPluginControl()
        {
            InitializeComponent();
        }

        public TebexPluginControl(TebexPlugin plugin) : this()
        {
            this.Plugin = plugin;
        }
        
        private void SaveConfig_OnClick(object sender, RoutedEventArgs e)
        {
            Plugin.SaveConfiguration();
        }
    }
}