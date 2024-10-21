using Microsoft.AspNetCore.SignalR;


namespace Airbnb.Application.Chatting
{
    public class ChatHub:Hub
    {
		public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, string username, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", username, message);
        }

    }
}
