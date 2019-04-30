using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Database.Entities
{
    public class AddressCityEty
    {
        public int Id { get; set; }
        public int CountyId { get; set; }
        public string Name { get; set; }        
    }
}
