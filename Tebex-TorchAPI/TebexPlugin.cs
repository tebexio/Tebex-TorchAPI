using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Tebex.Adapters;
using Tebex.API;
using Tebex.Shared.Components;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Commands;
using Torch.Session;
using VRage.FileSystem;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Plugins;

namespace TebexSpaceEngineersPlugin {

    //Notation by Bishbash777#0465
    public class TebexPlugin : TorchPluginBase
    {
        private static ITorchBase _torch;
        private Persistent<TebexTorchConfig> _config;
        public TebexTorchConfig Config => _config?.Data;
        
        #region Template
        
        // Torch initialization
        public override void Init(ITorchBase torchBase)
        {
            _torch = torchBase;
            var configPath = Path.Combine(StoragePath, "Tebex.cfg");
            _adapter.LogInfo($"Loading Tebex configuration from {configPath}");
            _config = Persistent<TebexTorchConfig>.Load(configPath);
            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            sessionManager.SessionStateChanged += OnSessionChanged;
        }
        
        public override void Update()
        {
            if (TebexSpaceEngineersAdapter.Plugin != null)
            {
                TebexSpaceEngineersAdapter.Plugin.Tick();
            }
        }

        //Run when server is in unload/shutdown
        public override void Dispose() {
            _adapter.ProcessJoinQueue();
        }

        //Returned to DS to display a friendly name of the plugin to the DS user...
        public string GetPluginTitle() {
            return "Tebex";
        }
        #endregion

        #region Tebex

        private static TebexSpaceEngineersAdapter _adapter;
         
        private static TickTimers _timers;
        private static WebRequests _webrequest;
        
        public static string GetPluginVersion()
        {
            return "2.0.0-DEV";
        }

        protected void SyncConfiguration()
        {
            // Sync configuration to BaseTebexAdapter model
            BaseTebexAdapter.PluginConfig.SecretKey = _config.Data.SecretKey;
            BaseTebexAdapter.PluginConfig.AutoReportingEnabled = _config.Data.AutoReportingEnabled;
            BaseTebexAdapter.PluginConfig.DebugMode = _config.Data.DebugMode;
            BaseTebexAdapter.PluginConfig.DisableRedeemCommand = _config.Data.DisableRedeemCommand;
        }

        private void OnSessionChanged(ITorchSession session, TorchSessionState state)
        {
            switch (state)
            {
                case TorchSessionState.Loaded:
                    SyncConfiguration();
                    _init();
                    break;
                case TorchSessionState.Unloading:
                    OnServerShutdown();
                    break;
            }
        }
        
        // Adapter initialization must take place when the game is running.
        private void _init()
        {
            // Setup our API and adapter
            _adapter = new TebexSpaceEngineersAdapter(this);
            _adapter.LogInfo("Tebex is starting up...");
            
            // Init plugin components so they have access to our adapter
            _webrequest = new WebRequests(_adapter);
            _timers = new TickTimers(_adapter);
            _timers.Once(10, () =>
            {
                SpaceEngineersCommands.InitItemDefinitions(_adapter);
            }); // run scheduled to allow errors to pass through without crashing the server
            
            TebexApi.Instance.InitAdapter(_adapter);

            // Check if auto reporting is disabled and show a warning if so.
            if (!BaseTebexAdapter.PluginConfig.AutoReportingEnabled)
            {
                _adapter.LogWarning("Auto reporting issues to Tebex is disabled.");
                _adapter.LogWarning("To enable, please set 'AutoReportingEnabled' to 'true' in config/Tebex.json");
            }

            // Check if secret key has been set. If so, get store information and place in cache
            if (BaseTebexAdapter.PluginConfig.SecretKey != "Your Tebex Secret Key")
            {
                _adapter.LogInfo("Secret key is set. Loading store info...");
                _adapter.FetchStoreInfo(info =>
                {
                    _adapter.LogInfo($"Connected to store {info.AccountInfo.Domain} as game server {info.ServerInfo.Name}");
                });
                return;
            }

            _adapter.LogInfo("Tebex did not find your secret key.");
            _adapter.LogInfo("Please set your game server key in the Dedicated Server Manager in the 'Plugins' tab. Select 'Tebex' from the list of plugins.");
            _adapter.LogInfo("Get your game server key from https://creator.tebex.io/game-servers");
        }

        public void Tick()
        {
            _timers.Update();
        }
        
        public WebRequests WebRequests()
        {
            return _webrequest;
        }

        public TickTimers PluginTimers()
        {
            return _timers;
        }

        public string GetGame()
        {
            return "Space Engineers - Torch";
        }

        public void OnUserConnected(MyPlayer player)
        {
            string playerIp = GetPlayerIp(player.Id.SteamId);
            _adapter.LogDebug($"Player login event: {player.Id} from " + playerIp);
            _adapter.OnUserConnected(player.Id.SteamId.ToString(), playerIp);
        }
        
        private void OnServerShutdown()
        {
            // Make sure join queue is always emptied on shutdown
            _adapter.ProcessJoinQueue();
        }

        private BaseTebexAdapter.TebexConfig GetDefaultConfig()
        {
            return new BaseTebexAdapter.TebexConfig();
        }

        public static BaseTebexAdapter GetAdapter()
        {
            return _adapter;
        }

        public void SaveConfiguration()
        {
            _config.Data.SecretKey = BaseTebexAdapter.PluginConfig.SecretKey;
            _config.Data.AutoReportingEnabled = BaseTebexAdapter.PluginConfig.AutoReportingEnabled;
            _config.Data.DebugMode = BaseTebexAdapter.PluginConfig.DebugMode;
            _config.Data.DisableRedeemCommand = BaseTebexAdapter.PluginConfig.DisableRedeemCommand;
            _config.Save();
        }

        public static string GetPlayerIp(ulong steamId)
        {
            var state = new MyP2PSessionState();
            MyGameService.Peer2Peer.GetSessionState(steamId, ref state);
            var ip = new IPAddress(BitConverter.GetBytes(state.RemoteIP).Reverse().ToArray());
            return ip.ToString();
        }
        
        public static void PrintCategories(IMyPlayer player, List<TebexApi.Category> categories)
        {
            // Index counter for selecting displayed items
            var categoryIndex = 1;
            var packIndex = 1;

            // Line separator for category response
            _adapter.ReplyPlayer(player, "---------------------------------");

            // Sort categories in order and display
            var orderedCategories = categories.OrderBy(category => category.Order).ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                var listing = orderedCategories[i];
                _adapter.ReplyPlayer(player, $"[C{categoryIndex}] {listing.Name}");
                categoryIndex++;

                // Show packages for the category in order from API
                if (listing.Packages.Count > 0)
                {
                    var packages = listing.Packages.OrderBy(category => category.Order).ToList();
                    _adapter.ReplyPlayer(player, $"Packages");
                    foreach (var package in packages)
                    {
                        // Add additional flair on sales
                        if (package.Sale != null && package.Sale.Active)
                        {
                            _adapter.ReplyPlayer(player,
                                $"-> [P{packIndex}] {package.Name} {package.Price - package.Sale.Discount} (SALE {package.Sale.Discount} off)");
                        }
                        else
                        {
                            _adapter.ReplyPlayer(player, $"-> [P{packIndex}] {package.Name} {package.Price}");
                        }

                        packIndex++;
                    }
                }

                // At the end of each category add a line separator
                _adapter.ReplyPlayer(player, "---------------------------------");
            }
        }

        public static void PrintPackages(IMyPlayer player, List<TebexApi.Package> packages)
        {
            // Index counter for selecting displayed items
            var packIndex = 1;

            _adapter.ReplyPlayer(player, "---------------------------------");
            _adapter.ReplyPlayer(player, "      PACKAGES AVAILABLE         ");
            _adapter.ReplyPlayer(player, "---------------------------------");

            // Sort categories in order and display
            var orderedPackages = packages.OrderBy(package => package.Order).ToList();
            for (var i = 0; i < packages.Count; i++)
            {
                var package = orderedPackages[i];
                // Add additional flair on sales
                _adapter.ReplyPlayer(player, $"[P{packIndex}] {package.Name}");
                _adapter.ReplyPlayer(player, $"Category: {package.Category.Name}");
                _adapter.ReplyPlayer(player, $"Description: {package.Description}");

                if (package.Sale != null && package.Sale.Active)
                {
                    _adapter.ReplyPlayer(player,
                        $"Original Price: {package.Price} {package.GetFriendlyPayFrequency()}  SALE: {package.Sale.Discount} OFF!");
                }
                else
                {
                    _adapter.ReplyPlayer(player, $"Price: {package.Price} {package.GetFriendlyPayFrequency()}");
                }

                _adapter.ReplyPlayer(player,
                    $"Purchase with 'tebex.checkout P{packIndex}' or 'tebex.checkout {package.Id}'");
                _adapter.ReplyPlayer(player, "--------------------------------");

                packIndex++;
            }
        }
        
        public static bool RunCommand(string command)
        {
            var manager = _torch.CurrentSession.Managers.GetManager<CommandManager>();
            var commandMessages = manager?.HandleCommandFromServer(command);
            _adapter.LogDebug($"response from command: '{command}'");
            if (commandMessages != null)
            {
                foreach (var commandMessage in commandMessages)
                {
                    _adapter.LogDebug($"- {commandMessage}");
                }    
            }
            else
            {
                _adapter.LogDebug("$- null");
            }
            
            return true; // assumes success FIXME
        }
        #endregion
    }
}
