namespace Example.Models
{
    public class OrderLine
    {
        public OrderLine() {}

        public OrderLine(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
