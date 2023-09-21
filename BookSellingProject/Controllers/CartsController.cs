using BookSellingProject.Data;
using BookSellingProject.Models.Domain;
using BookSellingProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;

namespace BookSellingProject.Controllers
{

    public class CartsController : Controller
    {
        private readonly BookProjectDbContext context;
        private readonly IToastNotification toastNotification;
        private readonly IHttpContextAccessor contextAccessor;

        public CartsController(BookProjectDbContext context,
            IToastNotification toastNotification,
            IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.toastNotification = toastNotification;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {

            var cartsJson = contextAccessor.HttpContext.Session.GetString("Cart");

            if (cartsJson == null && string.IsNullOrEmpty(cartsJson))
            {
                return RedirectToAction("Index", "Home");
            }

            // List<>
            var carts = JsonConvert.DeserializeObject<List<CartItem>>(cartsJson);

            var cartViewModel = new List<CartViewModel>();

            foreach(var cart in carts) {
                cartViewModel.Add(new CartViewModel
                {
                    Id = cart.Id,
                    Book = context.Books.FirstOrDefault(b => b.Id == cart.BookId),
                    Quantity = cart.Quantity,
                    SubTotal = cart.SubTotal,
                });
            }

            return View(cartViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddCart(Guid id)
        {
            var book = context.Books.FirstOrDefault(book => book.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var cartJSON = contextAccessor.HttpContext.Session.GetString("Cart");

            if (cartJSON == null)
            {
                cartJSON = JsonConvert.SerializeObject(new List<CartItem>());
            }


            var cartsDomain = JsonConvert.DeserializeObject<List<CartItem>>(cartJSON);

            var existingBook = cartsDomain.FirstOrDefault(cart => cart.BookId == id);
            var quantity = 1;

            if (existingBook is null)
            {
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    Quantity = quantity,
                    SubTotal = quantity * book.Price,
                    BookId = book.Id,
                };
                cartsDomain.Add(cartItem);

                var existingCarts = JsonConvert.SerializeObject(cartsDomain);
                contextAccessor.HttpContext.Session.SetString("Cart", existingCarts);
                toastNotification.AddSuccessToastMessage("A Book is added successfully");

                return RedirectToAction("Index", "Carts");

            }

            quantity += existingBook.Quantity;

            existingBook.Quantity = quantity;
            existingBook.SubTotal *= existingBook.Quantity;

            var carts = JsonConvert.SerializeObject(cartsDomain);
            contextAccessor.HttpContext.Session.SetString("Cart", carts);


            return RedirectToAction("Index", "Carts");
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            var cartJSON = contextAccessor.HttpContext.Session.GetString("Cart");


            var cartsDomain = JsonConvert.DeserializeObject<List<CartItem>>(cartJSON);

            foreach (var cart in cartsDomain)
            {
                if (cart.Id == id)
                {
                    cartsDomain.Remove(cart);
                    break;
                }
            }

            var existingCarts = JsonConvert.SerializeObject(cartsDomain);
            contextAccessor.HttpContext.Session.SetString("Cart", existingCarts);
            toastNotification.AddSuccessToastMessage("A Book is deleted successfully");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid Id, int Quantity)
        {
            var cartJSON = contextAccessor.HttpContext.Session.GetString("Cart");


            if (cartJSON != null)
            {

                var cartsDomain = JsonConvert.DeserializeObject<List<CartItem>>(cartJSON);
                int subTotal = 0;

                // Id san pham = so luong hien tai
                foreach (var cart in cartsDomain)
                {
                    if (cart.Id == Id)
                    {
                        cart.Quantity = Quantity;
                        var book = context.Books.FirstOrDefault(x => x.Id == cart.BookId);
                        if (book != null)
                        {
                            cart.SubTotal = cart.Quantity * book.Price;
                            subTotal = cart.SubTotal;
                        }
                    }
                }
                // Cap nhat
                var existingCarts = JsonConvert.SerializeObject(cartsDomain);
                contextAccessor.HttpContext.Session.SetString("Cart", existingCarts);

                return Ok(new
                {
                    SubTotal = subTotal,
                    Total = cartsDomain.Sum(c => c.SubTotal)
                });
            }

            return RedirectToAction("Index", "Carts");

        }
    }
}
