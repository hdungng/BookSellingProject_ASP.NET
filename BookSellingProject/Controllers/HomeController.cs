using BookSellingProject.Data;
using BookSellingProject.Models;
using BookSellingProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookSellingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookProjectDbContext context;

        public HomeController(ILogger<HomeController> logger, BookProjectDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await context.Categories.ToListAsync();
            var books = await context.Books.ToListAsync();

            var categoryBooksViewModel = new CategoryBooksViewModel();
            categoryBooksViewModel.Categories = categories;
            categoryBooksViewModel.Books = books;


            return View(categoryBooksViewModel);
        }


        public async Task<IActionResult> Category(Guid id)
        {
            var books = await context.Books.Where(book => book.CategoryId == id)
                .OrderByDescending(book => book.CreatedAt)
                .ToListAsync();

            var categories = await context.Categories.ToListAsync();

            var currentCategoryName = categories.FirstOrDefault(c => c.Id == id).Name;


            var categoryBooksViewModel = new CategoryBooksViewModel();

            categoryBooksViewModel.BooksQuantity = books.Count;
            categoryBooksViewModel.CurrentCategoryName = currentCategoryName;
            categoryBooksViewModel.Categories = categories;
            categoryBooksViewModel.Books = books;


            return View(categoryBooksViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var books = await context.Books.Where(b => b.Name.Contains(keyword))
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync(); 

            return View(books);

        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if(book == null)
            {
                return View("Error");
            }

            return View(book);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}