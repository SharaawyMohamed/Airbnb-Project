using Airbnb.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Infrastructure.Specifications
{
	public class NotificationsWithSpeck : BaseSpecifications<Notification, int>
	{
		public NotificationsWithSpeck(NotificationParam param) : base(b => b.UserId == param.userId)
		{
			AddOrderByDescending(x => x.CreatedAt);

			int skip = (param.pageIndex - 1) * (param.PageSize);
			skip = skip > 0 ? skip : 0;

			int take = Math.Max(param.PageSize,5);

			ApplyPagination(skip, take);
		}

		public NotificationsWithSpeck(string userId, bool IsRead) : base(B =>
		(!string.IsNullOrWhiteSpace(userId) || B.UserId == userId) &&
		(B.IsRead == IsRead))
		{
		}

	}
}
