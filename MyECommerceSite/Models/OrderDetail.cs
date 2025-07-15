namespace MyECommerceSite.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign Key
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // ფასი შეკვეთის მომენტში

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}