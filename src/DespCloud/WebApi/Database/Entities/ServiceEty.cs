namespace WebApi.Database.Entities
{
    public class ServiceEty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Honorary { get; set; }
        public decimal Rate { get; set; }
        public decimal PlateCard { get; set; }
        public decimal Other { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }
    }
}