namespace Base.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string RgIE { get; set; }
        public string Org { get; set; }

        public Address Address { get; set; }
    }
}