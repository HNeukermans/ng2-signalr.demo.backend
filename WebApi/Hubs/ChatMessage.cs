using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ng2SignalR.hub.Hubs
{
    public class ChatMessage
    {
        public string content;
        public string user;

        public ChatMessage(string user, string message)
        {
            this.user = user;
            this.content = message;
        }
    }
}
