using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Database.Entities
{
    public class ContextSequenceEty
    {        
        public string Context { get; set; }
        public int Value { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }
    }
}
