using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class Address
    {
        public string ZipCode { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public string CountyInitials { get; set; }
    }
}
