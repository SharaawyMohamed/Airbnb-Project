using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Interfaces.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task SendMessageBroadcast(string ReceiveMessage,string username,string message);
    }
}
