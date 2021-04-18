using Licenta.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.Hubs
{
    public class ChatHub : Hub
    {
        // get connection id
        public string GetConnectionId() => Context.ConnectionId;

        // send messages to all clients
        public async Task SendMessage(Message message) =>
            await Clients.All.SendAsync("receiveMessage", message);
    }
}
