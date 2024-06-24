using Application.Contracts;
using Application.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repo
{
   public class NotificationsHub : Hub
    {
        

        public async Task NotifyDonor(string requestId, string donorId)
         {
             await Clients.User(donorId).SendAsync("ReceiveNotification", requestId);
         }

        public async Task RespondToRequest(string requestId, bool response)
        {
            await Clients.Group($"request-{requestId}").SendAsync("ReceiveResponse", response);

            // Notify donor about the response
            await Clients.User(Context.UserIdentifier).SendAsync("ReceiveDonorResponse", requestId, response);

            // If accepted, send notification to requester
            if (response)
            {
                await Clients.Group($"request-{requestId}").SendAsync("RequestAccepted", requestId);
            }
        }

         public async Task JoinRequestGroup(string requestId)
         {
             await Groups.AddToGroupAsync(Context.ConnectionId, $"request-{requestId}");
         }


    }
}
