﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.BookingToPayment.Entities
{
    public class BookingToPayment
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal PropertyNightPrice { get; set; }
        public string BookingType { get; set; }
        public string PropertyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
