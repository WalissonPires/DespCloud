namespace Base.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Plate { get; set; }
        public string Chassis { get; set; }
        public string Renavam { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int YearManufacture { get; set; }
        public int ModelYear { get; set; }
        public VehicleType Type { get; set; }
        public string Color { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? CountyId { get; set; }
        public string CountyName { get; set; }
        public string CountyInitials { get; set; }

        public Client Client { get; set; }
    }
}