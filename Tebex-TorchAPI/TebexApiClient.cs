using System;
using System.Net;
using TebexTorchAPI.Commands;
using Newtonsoft.Json.Linq;

namespace TebexTorchAPI
{
    public class TebexApiClient : WebClient
    {

        //time in milliseconds
        private Tebex plugin;
        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        public void setPlugin(Tebex plugin)
        {
            this.plugin = plugin;
        }

        public TebexApiClient(int timeout = 5000)
        {
            this.timeout = timeout;
        }

        public void DoGet(string endpoint, TebexCommandModule command)
        {
            this.Headers.Add("X-Buycraft-Secret", Tebex.Instance.Config.Secret);
            String url = Tebex.Instance.Config.BaseUrl + endpoint;
            Tebex.logWarning("GET " + url);
            this.DownloadStringCompleted += (sender, e) =>
            {
                if (!e.Cancelled && e.Error == null)
                {
                    command.HandleResponse(JObject.Parse(e.Result));    
                }
                else
                {
                    Tebex.logWarning(e.Error.ToString());
                    command.HandleError(e.Error);
                }
                this.Dispose();
            };
            this.DownloadStringAsync(new Uri(url));
        }
             
    }
}