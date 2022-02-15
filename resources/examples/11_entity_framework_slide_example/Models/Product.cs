using System.ComponentModel.DataAnnotations;

namespace Example.Models
{
    public class Product
    {
        public Product() {}

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
