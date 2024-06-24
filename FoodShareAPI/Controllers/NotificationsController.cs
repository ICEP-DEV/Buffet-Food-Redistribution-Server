using Infrastructure.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FoodShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        public NotificationsController(IHubContext<NotificationsHub> context) 
        {
            this._hubContext = context;
        }

       [HttpPost("respond")]

       public async Task <IActionResult> RespondToRequest(string requestId, bool response)
        {
            try
            {
                if(response)
                {
                    Console.WriteLine($"Request has been accepted");
                }
                else
                {
                    Console.WriteLine("Request has been declined");
                }

                await _hubContext.Clients.All.SendAsync("ReceiveResponse", requestId, response);
                return Ok();
            }
            catch(Exception ex) 
            {
                return StatusCode(500, $"Error responding to request: {ex.Message}");
            }
        }

       
    }
}
