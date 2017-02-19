using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ng2SignalR.hub.Hubs
{
    [HubName("Ng2SignalRHub")]
    public class Ng2SignalRHub : Hub
    {
        public override Task OnConnected() {

            Trace.TraceInformation("Ng2SignalRHub - OnConnected");

            var user = GetAuthenticatedUser();
                        
            Clients.All.OnUserConnected(user);

            return base.OnConnected();
        }

        public void Chat(ChatMessage message)
        {
            Trace.TraceInformation("Ng2SignalRHub - Chat");

            Clients.All.OnMessageSent(message);
        }

        public List<string> GetNgBeSpeakers()
        {
            Trace.TraceInformation("Ng2SignalRHub - GetNgBeSpeakers");

            return new List<string>() { "Igor", "Tero", "Todd", "Pascal", "Pawel", "Gerard" };
        }

        public List<string> GetNgBeCoreTeam(Parameters parameters)
        {
            Trace.TraceInformation("Ng2SignalRHub - GetNgBeCoreTeam");

            return new List<string>() { "Hannes", "Tim", "Jurgen", "Sam", "Kwinten", "Brecht" };
        }

        public List<string> ThrowException()
        {
            throw new System.Exception("Oops, an error occured");
        }
             

        public override Task OnDisconnected(bool stopCalled)
        {
            Trace.TraceInformation("Ng2SignalRHub - OnDisconnected");

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Trace.TraceInformation("Ng2SignalRHub - OnReconnected");

            return base.OnReconnected();
        }

        private string GetAuthenticatedUser() {
            var username = Context.QueryString["user"];
           
            Trace.TraceInformation("GetAuthenticatedUser :" + username);

            return username;
        }

        public Task JoinRoom()
        {
            return Groups.Add(Context.ConnectionId, "Room");
        }

        public Task LeaveRoom()
        {
            return Groups.Remove(Context.ConnectionId, "Room");
        }

        public void ChatInRoom(ChatMessage message)
        {
            if (message.content == "") return; // HACK: echo-cancelation 
            Clients.OthersInGroup("Room").OnRoomMessageSent(message);
        }

        public void KeyupInRoom(string status)
        {
            Clients.OthersInGroup("Room").OnRoomKeyupSent(status);
        }
    }

    public class Parameters {
        public int ANumber { get; set; }
        public string AString { get; set; }
        public List<string> AList { get; set; }
    }
}
