namespace MyECommerceSite.Models
{
    public class Product
    {
        public int Id { get; set; } // პროდუქტის უნიკალური ID
        public string Name { get; set; } // პროდუქტის სახელი
        public string Description { get; set; } // მოკლე აღწერა
        public decimal Price { get; set; } // ფასი
        public string ImageUrl { get; set; }
    }
}
