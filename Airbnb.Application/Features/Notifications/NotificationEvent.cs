using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications
{
	public class NotificationEvent : INotification
	{
		public string Message { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public bool IsPublic { get; set; }
	}
}
