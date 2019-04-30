namespace WebApi.Database.Entities
{
    public class OrderDetailEty
    {
        public int Id { get; set; }
        public decimal Honorary { get; set; }
        public decimal Rate { get; set; }
        public decimal PlateCard { get; set; }
        public decimal Other { get; set; }
        public decimal Total { get; set; }
        public int CompanyId { get; set; }        

        public int VehicleId { get; set; }
        public virtual VehicleEty Vehicle { get; set; }

        public int OrderId { get; set; }
        public virtual OrderServiceEty Order { get; set; }

        public int ServiceId { get; set; }
        public virtual ServiceEty Service { get; set; }
    }
}