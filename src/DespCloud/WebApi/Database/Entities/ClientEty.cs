using System;

namespace WebApi.Database.Entities
{
    public class ClientEty
    {
        public int Id { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string RgIE { get; set; }
        public string Org { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }

        public int? AddressId { get; set; }
        public virtual AddressEty Address { get; set; }
    }
}