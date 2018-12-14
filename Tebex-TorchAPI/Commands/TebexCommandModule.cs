using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;

namespace TebexTorchAPI.Commands
{
    public abstract class TebexCommandModule : CommandModule
    {
        public abstract void HandleResponse(JObject response);

        public abstract void HandleError(Exception e);
    }
}
