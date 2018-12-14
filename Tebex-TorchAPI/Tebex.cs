using System;
using System.IO;
using System.Windows.Controls;
using TebexTorchAPI.Commands;
using NLog;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Plugins;
using Torch.API.Session;
using Torch.Session;
using Torch.Views;
using TebexTorchAPI.Models;
using Torch.Commands;

namespace TebexTorchAPI
{
    public class Tebex : TorchPluginBase, IWpfPlugin
    {
        public TebexConfig Config => _config?.Data;

        private TorchSessionManager _sessionManager;

        private UserControl _control;
        private Persistent<TebexConfig> _config;
        private static readonly Logger Log = LogManager.GetLogger("TebexTorchAPI");

        private DateTime lastCalled = DateTime.Now.AddMinutes(-14);
        public int nextCheck = 15 * 60;
        public WebstoreInfo information;

        private System.Timers.Timer aTimer;


        public static Tebex Instance { get; private set; }

        /// <inheritdoc />
        public UserControl GetControl() => _control ?? (_control = new PropertyGrid(){DataContext=Config, IsEnabled = false});

        public void Save()
        {
            _config.Save();
        }

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            string path = Path.Combine(StoragePath, "TebexTorchAPI.cfg");
            Tebex.logInfo($"Attempting to load config from {path}");
            _config = Persistent<TebexConfig>.Load(path);
            _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (_sessionManager != null)
                _sessionManager.SessionStateChanged += SessionChanged;

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                            (sender, certificate, chain, errors) => { return true; };

            Torch.GameStateChanged += GameStateChanged;

            this.information = new WebstoreInfo();
            Instance = this;
        }

        private void checkQueue(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (Instance.Config.Secret == "")
            {
                logError("You have not yet defined your secret key. Use !tebex:secret <secret> to define your key");
            }else if ((DateTime.Now - this.lastCalled).TotalSeconds > Tebex.Instance.nextCheck)
            {
                this.lastCalled = DateTime.Now;
                TebexForcecheckModule checkCommand = new TebexForcecheckModule();
                checkCommand.TebexForcecheck();
            }
        }


        private void GameStateChanged(Sandbox.MySandboxGame game, TorchGameState newState)
        {
            if (newState == TorchGameState.Loaded)
            {
                
            }

            if (newState == TorchGameState.Unloading)
            {

            }
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state)
        {
            
            switch (state)
            {
                case TorchSessionState.Loaded:
                    Tebex.logInfo($"Game started....");

                    if (Instance.Config.Secret == "")
                    {
                        logError("You have not yet defined your secret key. Use !tebex:secret <secret> to define your key");
                    }
                    else
                    {
                        TebexInfoModule infoCommand = new TebexInfoModule();
                        infoCommand.TebexInfo();
                    }

                    // Create a timer with a two second interval.
                    aTimer = new System.Timers.Timer(60000);
                    // Hook up the Elapsed event for the timer. 
                    aTimer.Elapsed += checkQueue;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;

                    
                    break;
                case TorchSessionState.Unloading:
                    Tebex.logInfo($"Game ending....");
                    aTimer.Enabled = false;
                    aTimer.Dispose();
                    break;                    
            }
        }

        public static void logWarning(String message)
        {
            Log.Warn(message);
        }

        public static void logError(String message)
        {
            Log.Error(message);
        }

        public static void logInfo(String message)
        {
            Log.Info(message);
        }



        public override void Update()
        {
            
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            
        }
    }
}
