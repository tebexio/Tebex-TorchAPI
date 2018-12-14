using Torch;
using Torch.Views;

namespace TebexTorchAPI
{
    public class TebexConfig : ViewModel
    {
        public TebexConfig()
        {            
        }

        private bool _buyEnabled = true;
        [Display(Name = "Enable Buy Menu", Description = "Enable the Buy Meu")]
        public bool BuyEnabled { get => _buyEnabled; set => SetValue(ref _buyEnabled, value); }

        private string _secret = "";
        [Display(Name = "Secret Key", Description = "Your server secret key")]
        public string Secret { get => _secret; set => SetValue(ref _secret, value); }

        private string _buyCommand = "!donate";
        [Display(Name = "Buy Command", Description = "The command players use to bring up the buy menu")]
        public string BuyCommand { get => _buyCommand; set => SetValue(ref _buyCommand, value); }

        private string _baseUrl = "https://plugin.buycraft.net/";
        [Display(Name = "Base URL", Description = "The base API url")]
        public string BaseUrl { get => _baseUrl; set => SetValue(ref _baseUrl, value); }

    }
}
