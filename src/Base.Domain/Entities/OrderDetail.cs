namespace Base.Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ServiceId { get; set; }
        public int VehicleId { get; set; }
        public Service Service { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal Total => Service == null ? 0 : (Service.Honorary + Service.Rate + Service.PlateCard + Service.Other);
    }
}