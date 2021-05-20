using Microsoft.AspNetCore.SignalR;

namespace VPMS_Project.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
        
    }
}