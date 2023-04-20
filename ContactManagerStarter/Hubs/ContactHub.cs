using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ContactManager.Data;

namespace ContactManager.Hubs
{
    public class ContactHub : Hub
    {
        public async Task Update(Contact newContact)
        {
            await Clients.All.SendAsync("Update", newContact);
        }
    }
}
