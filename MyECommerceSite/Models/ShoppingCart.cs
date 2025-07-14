namespace MyECommerceSite.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.Product.Id == product.Id);

            if (existingItem == null)
            {
                Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var itemToRemove = Items.FirstOrDefault(i => i.Product.Id == productId);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
            }
        }

        public decimal GetTotal()
        {
            return Items.Sum(i => i.Product.Price * i.Quantity);           
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
