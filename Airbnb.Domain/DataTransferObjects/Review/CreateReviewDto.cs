using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects.Review
{
	public class CreateReviewDto
	{
		public string UserId { get; set; } = string.Empty;
		public string PropertyId { get; set; }= string.Empty;
		public string Comment { get; set; } = string.Empty;
		public int Stars { get; set; }
	}
}
