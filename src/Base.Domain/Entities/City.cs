using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public int CountyId { get; set; }
        public string Name { get; set; }
    }
}
