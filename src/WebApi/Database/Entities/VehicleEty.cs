namespace WebApi.Database.Entities
{
    public class VehicleEty
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Chassis { get; set; }
        public string Renavam { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public int? YearManufacture { get; set; }
        public int? ModelYear { get; set; }
        public VehicleTypeEty Type { get; set; }
        public string Color { get; set; }       
        public string CityName { get; set; }        
        public string CountyName { get; set; }
        public string CountyInitials { get; set; }

        public int CompanyId { get; set; }
        public virtual CompanyEty Company { get; set; }

        public int ClientId { get; set; }
        public virtual ClientEty Client { get; set; }

        public int? CityId { get; set; }        
        public virtual AddressCityEty City { get; set; }

        public int? CountyId { get; set; }
        public virtual AddressCountyEty County { get; set; }
    }
}