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

    }
}
