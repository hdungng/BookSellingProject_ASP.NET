using BookSellingProject.Data;
using BookSellingProject.Models.Domain;
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

            if(cartsJson == null && string.IsNullOrEmpty(cartsJson))
            {
                return RedirectToAction("Index", "Home");
            }

            // List<>
            var carts = JsonConvert.DeserializeObject<List<CartItem>>(cartsJson);

            return View(carts);
        }

        [HttpGet]
        public async Task<IActionResult> AddCart(Guid id)
        {
            var book = context.Books.FirstOrDefault(book => book.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            // check xem sản phẩm đó có tồn tại hay chưa? nếu có rồi, tăng số lượng SP 
            var cartJSON = contextAccessor.HttpContext.Session.GetString("Cart");

            // List<CartItem> cartsDomain 
            // nếu chưa có giỏ hàng, tạo giỏ hàng
            if(cartJSON == null)
            {
                cartJSON = JsonConvert.SerializeObject(new List<CartItem>());
            }


            var cartsDomain = JsonConvert.DeserializeObject<List<CartItem>>(cartJSON);

            // check if a Book in CartItem is existed or NOT
            var existingBook = cartsDomain.FirstOrDefault(cart => cart.Book.Id == id);
            var quantity = 1;

            if (existingBook is null)
            {
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    Quantity = quantity,
                    SubTotal = quantity * book.Price,
                    Book = book,
                };
                cartsDomain.Add(cartItem);

                // Add in Session
                var existingCarts = JsonConvert.SerializeObject(cartsDomain);
                contextAccessor.HttpContext.Session.SetString("Cart", existingCarts);
                toastNotification.AddSuccessToastMessage("A Book is added successfully");

                return RedirectToAction("Index", "Carts");

            }

            quantity += existingBook.Quantity;

            existingBook.Quantity = quantity;
            existingBook.SubTotal *= existingBook.Quantity;

            // Add in Session
            var carts = JsonConvert.SerializeObject(cartsDomain);
            contextAccessor.HttpContext.Session.SetString("Cart", carts);


            return RedirectToAction("Index", "Carts");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            //xóa sản phẩm có $id trong giỏ hàng
            var cartJSON = contextAccessor.HttpContext.Session.GetString("Cart");

            // List<CartItem> cartsDomain 
            // nếu chưa có giỏ hàng, tạo giỏ hàng
            if (cartJSON == null)
            {
                cartJSON = JsonConvert.SerializeObject(new List<CartItem>());
            }
            
            return RedirectToAction("Index");
        }
    }
}
