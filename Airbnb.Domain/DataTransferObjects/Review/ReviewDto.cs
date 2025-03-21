﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects.Review
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public string PropertyId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Stars { get; set; }

    }
}
