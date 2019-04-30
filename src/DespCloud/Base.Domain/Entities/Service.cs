namespace Base.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Honorary { get; set; }
        public decimal Rate { get; set; }
        public decimal PlateCard { get; set; }
        public decimal Other { get; set; }
    }
}