﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Services.Models
{
    public class Location
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZIP { get; set; }
    }
}
