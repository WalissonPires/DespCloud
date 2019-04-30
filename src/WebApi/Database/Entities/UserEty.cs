using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Database.Entities
{
    public class UserEty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }
    }
}
