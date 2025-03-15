using Airbnb.Application.Features.Notifications.Commands;
using Airbnb.Application.Features.Notifications.Query.CheckNewNotifications;
using Airbnb.Application.Features.Notifications.Query.GetAllNotificationsWithpagination;
using Airbnb.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
	public class NotificationController : APIBaseController
	{
		private readonly IMediator _mediator;

		public NotificationController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("GetPaginatedNotifications")]
		public async Task<ActionResult<Responses>> GetPaginatedNotifications([FromQuery] GetPaginatedNotificationsQuery query)
		{
			return Ok(await _mediator.Send(query));
		}

		[HttpGet("CheckNewNotifications")]
		public async Task<ActionResult<Responses>> CheckNewNotifications()
		{
			var query = new GetNewNotificationsQuery();
			return Ok(await _mediator.Send(query));
		}

		[HttpPut("MakeNotificationAsReading")]
		public async Task<ActionResult<Responses>> MakeNotificationAsReading([FromForm] MakNotificationRed command)
		{
			return Ok(await _mediator.Send(command));
		}

		[HttpDelete("DeleteNotification")]
		public async Task<ActionResult<Responses>> DeleteNotification([FromForm] DeleteNotificationCommand command)
		{
			return Ok(await _mediator.Send(command));
		}

	}
}
