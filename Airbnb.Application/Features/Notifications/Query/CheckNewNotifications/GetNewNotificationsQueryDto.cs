using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications.Query.CheckNewNotifications
{
	public class GetNewNotificationsQueryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; }
		public string UserId { get; set; }
	}
}
