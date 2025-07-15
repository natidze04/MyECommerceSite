using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyECommerceSite.Data;
using MyECommerceSite.Infrastructure;
using MyECommerceSite.Models;
using System.Threading.Tasks;

namespace MyECommerceSite.Controllers
{
    [Authorize] // მხოლოდ ავტორიზებული მომხმარებლები შეძლებენ შეკვეთას
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Order/Checkout
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if (cart.Items.Count == 0)
            {
                TempData["Message"] = "Your cart is empty. Please add products before checking out.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            return View(new Order()); // ვაბრუნებთ ცარიელ ფორმას
        }

        // POST: /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            ModelState.Remove("UserId");
            var cart = HttpContext.Session.GetJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            if (cart.Items.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty.");
            }

            if (ModelState.IsValid)
            {
                // შევავსოთ შეკვეთის დანარჩენი ველები
                order.UserId = _userManager.GetUserId(User);
                order.OrderDate = DateTime.Now;
                order.OrderTotal = cart.GetTotal();

                // დავამატოთ პროდუქტები შეკვეთას
                foreach (var item in cart.Items)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity,
                        Price = item.Product.Price // ფიქსირდება ფასი შეკვეთის მომენტში
                    });
                }

                // შევინახოთ ბაზაში
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // გავასუფთავოთ კალათა
                HttpContext.Session.Remove("Cart");

                // გადავამისამართოთ მადლობის გვერდზე
                return RedirectToAction("Complete", new { id = order.Id });
            }

            return View(order); // თუ მოდელი არაა ვალიდური, დავაბრუნოთ ფორმა შეცდომებით
        }

        public IActionResult Complete(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}