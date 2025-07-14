// Controllers/ShoppingCartController.cs
using Microsoft.AspNetCore.Mvc;
using MyECommerceSite.Data;
using MyECommerceSite.Infrastructure;
using MyECommerceSite.Models;
using System.Linq;

namespace MyECommerceSite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // აჩვენებს კალათის გვერდს
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // ამატებს პროდუქტს კალათაში
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, quantity);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        // შლის პროდუქტს კალათიდან
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // დამხმარე მეთოდი კალათის Session-დან წამოსაღებად
        private ShoppingCart GetCart()
        {
            ShoppingCart cart = HttpContext.Session.GetJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return cart;
        }

        // დამხმარე მეთოდი კალათის Session-ში შესანახად
        private void SaveCart(ShoppingCart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}