﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class District
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
    }
}
