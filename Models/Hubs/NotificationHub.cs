using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace UkrStanko.Models.Hubs
{
    public class NotificationHub: Hub
    { 
        public async Task Send(string message)
        {
            var userName = Context.User.Identity.Name;

            await Clients.All.SendAsync("Recieve", message, Context.User.Identity.Name);
        } // Send
    }
}
