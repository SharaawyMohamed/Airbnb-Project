using Airbnb.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.Entities
{
	public class Notification : BaseEntity<int>
	{
		public DateTime CreatedAt { get; set; }
		public bool IsRead { get; set; }
		public string UserId { get; set; }
		public virtual AppUser User { get; set; }
	}
}
