using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Infrastructure.Specifications
{
	public class NotificationParam
	{
		public string userId { get; set; }
		public int pageIndex { get; set; }
		private int pageSize = 20;
		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = (value > 100 || pageSize < 1) ? 20 : value; }
		}
	}
}
