using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Rea_Time
{
	public class UserConnectionManager
	{
		private readonly ConcurrentDictionary<string, string> _userConnections = new();

		public void AddUserConnection(string userId, string connectionId)
		{
			_userConnections[userId] = connectionId;
		}

		public void RemoveUserConnection(string connectionId)
		{
			var item = _userConnections.FirstOrDefault(x => x.Value == connectionId);
			if (item.Key != null)
			{
				_userConnections.TryRemove(item.Key, out _);
			}
		}

		public string GetConnectionId(string userId)
		{
			_userConnections.TryGetValue(userId, out var connectionId);
			return connectionId!;
		}
	}
}
