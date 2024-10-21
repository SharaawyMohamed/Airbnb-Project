using Airbnb.Domain;
using Castle.Core.Logging;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications
{
	public record CreatePropertyNotification(string PropertyId):INotification;
	public class SendNotificationForCreatePropertyHandler : INotificationHandler<CreatePropertyNotification>
	{
		private readonly ILogger<SendNotificationForCreatePropertyHandler> _logger;

		public SendNotificationForCreatePropertyHandler(ILogger<SendNotificationForCreatePropertyHandler> logger)
		{
			_logger = logger;
		}

		public async  Task Handle(CreatePropertyNotification notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Property with Id : {@PropertyId}, has been created successfully.",notification.PropertyId);
			await Task.Delay(2000);
			_logger.LogInformation("Creation of property notification sent {@PropertyId}", notification.PropertyId);
		}
	}
}
