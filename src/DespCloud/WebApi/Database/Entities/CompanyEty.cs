namespace WebApi.Database.Entities
{
    public class CompanyEty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LogoPath { get; set; }

        public int? AddressId { get; set; }
        public virtual AddressEty Address { get; set; }
    }
}