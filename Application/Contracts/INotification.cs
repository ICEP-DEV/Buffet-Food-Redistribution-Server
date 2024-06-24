using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface INotification
    {
        Task NotifyDonor(string requestId, string donorId);
        Task RespondToRequest(string requestId, bool response);
    }
}
