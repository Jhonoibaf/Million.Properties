
namespace Million.Properties.Application.DTOs

{
    public class CreatePropertyDto
    {
        public string _id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int InternalCode { get; set; }
        public int Year { get; set; }
        public int IdOwner { get; set; }
        public string File { get; set; } = string.Empty;
    }
}
