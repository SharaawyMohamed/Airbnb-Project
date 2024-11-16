using Microsoft.AspNetCore.SignalR;

namespace Airbnb.Application.Rea_Time
{
	public class NotificationHub : Hub
	{
		private readonly UserConnectionManager _userConnectionManager;

		public NotificationHub(UserConnectionManager userConnectionManager)
		{
			_userConnectionManager = userConnectionManager;
		}

		public override async Task OnConnectedAsync()
		{
			var userId = Context.UserIdentifier;
			var connectionId = Context.ConnectionId;

			_userConnectionManager.AddUserConnection(userId!, connectionId);

			await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);

			await base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			_userConnectionManager.RemoveUserConnection(Context.ConnectionId);
			return base.OnDisconnectedAsync(exception);
		}

	}
}
