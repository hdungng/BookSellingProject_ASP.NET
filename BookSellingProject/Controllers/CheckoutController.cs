using BookSellingProject.Data;
using BookSellingProject.Models.Domain;
using BookSellingProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NToastNotify;

namespace BookSellingProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly BookProjectDbContext context;
        private readonly IToastNotification toastNotification;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly BookProjectDbContext bookProjectDbContext;

        public CheckoutController(BookProjectDbContext context,
            IToastNotification toastNotification,
            IHttpContextAccessor contextAccessor, 
            BookProjectDbContext bookProjectDbContext)
        {
            this.context = context;
            this.toastNotification = toastNotification;
            this.contextAccessor = contextAccessor;
            this.bookProjectDbContext = bookProjectDbContext;
        }

        public IActionResult Index()
        {
            var cartsJson = contextAccessor.HttpContext.Session.GetString("Cart");

            if (cartsJson == null && string.IsNullOrEmpty(cartsJson))
            {
                return RedirectToAction("Index", "Home");
            }

            // List<>
            var carts = JsonConvert.DeserializeObject<List<CartItem>>(cartsJson);

            var cartViewModel = new List<CartViewModel>();

            foreach (var cart in carts)
            {
                cartViewModel.Add(new CartViewModel
                {
                    Id = cart.Id,
                    Book = context.Books.FirstOrDefault(b => b.Id == cart.BookId),
                    Quantity = cart.Quantity,
                    SubTotal = cart.SubTotal,
                });
            }

            var checkoutViewModel = new CreateCheckoutViewModel
            {
                CartItem = cartViewModel
            };

            return View(checkoutViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CreateCheckoutViewModel createCheckoutViewModel)
        {
            var cartsJson = contextAccessor.HttpContext.Session.GetString("Cart");

            if (cartsJson == null && string.IsNullOrEmpty(cartsJson))
            {
                return RedirectToAction("Index", "Home");
            }

            var carts = JsonConvert.DeserializeObject<List<CartItem>>(cartsJson);


            var checkoutDomain = new Invoice
            {
                Id = Guid.NewGuid(),
                Name = createCheckoutViewModel.Name,
                Address = createCheckoutViewModel.Address,
                Email = createCheckoutViewModel.Email,
                Telephone = createCheckoutViewModel.Telephone,
                Note = createCheckoutViewModel.Note,
                Total = createCheckoutViewModel.Total,
                CartItems = carts,
                CreatedAt = DateTime.Now,
            };

            await bookProjectDbContext.Invoices.AddAsync(checkoutDomain);
            
            await bookProjectDbContext.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("Thank you for your order! Your order is created successfully");

            contextAccessor.HttpContext.Session.SetString("Cart", "");
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var invoices = await context.Invoices
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(invoices);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(Guid id)
        {
            var invoice = await context.Invoices.
                Include(i => i.CartItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            var cartViewModel = new List<CartViewModel>();

            foreach (var cart in invoice.CartItems)
            {
                cartViewModel.Add(new CartViewModel
                {
                    Id = cart.Id,
                    Book = context.Books.FirstOrDefault(b => b.Id == cart.BookId),
                    Quantity = cart.Quantity,
                    SubTotal = cart.SubTotal,
                });
            }

            var invoiceViewModel = new InvoiceViewModel
            {
                Id = invoice.Id,
                Name = invoice.Name,
                Address = invoice.Address,
                CreatedAt = invoice.CreatedAt,
                Email = invoice.Email,
                Note = invoice.Note,
                Telephone = invoice.Telephone,
                Total = invoice.Total,
                CartItems = cartViewModel
            };

            if (invoice == null)
                return View("Error");


            return View(invoiceViewModel);
        }
    }
}
