using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSellingProject.Data;
using BookSellingProject.Models.Domain;
using BookSellingProject.Models.ViewModels;
using Azure;
using NToastNotify;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace BookSellingProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly BookProjectDbContext _context;
        private readonly IToastNotification toastNotification;

        public BooksController(BookProjectDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? page)
        {
            if (page == null) page = 1;

            int pageSize = 5;

            int pageNumber = (page ?? 1);

 
            var books = await _context.Books.Include(b => b.Category)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();


            return View(books.ToPagedList(pageNumber, pageSize));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            var model = new CreateBookViewModel
            {
                Categories = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookViewModel createBookViewModel)
        {
            if (ModelState.IsValid)
            {
                var bookDomainModel = new Book
                {
                    Author = createBookViewModel.Author,
                    Name = createBookViewModel.Name,
                    Price = createBookViewModel.Price,
                    Image = createBookViewModel.Image,
                    ShortDescription = createBookViewModel.ShortDescription,
                    Content = createBookViewModel.Content,
                    CreatedAt = DateTime.Now,
                    CategoryId = createBookViewModel.SelectedCategory,
                };

                await _context.Books.AddAsync(bookDomainModel);
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("A Book is created successfully");
                return RedirectToAction("Index");
            }

            toastNotification.AddErrorToastMessage("An error occured when creating a Book");
            return View(createBookViewModel);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(x => x.Id == id);
            var selectedCategory = await _context.Categories.ToListAsync();
            if (book == null)
            {
                return View("Index");
            }

            var editBookViewModel = new EditBookViewModel
            {
                Name = book.Name,
                Author = book.Author,
                Price = book.Price,
                Image = book.Image,
                ShortDescription = book.ShortDescription,
                Content = book.Content,
                Categories = selectedCategory.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                SelectedCategory = book.CategoryId
            };

            return View(editBookViewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditBookViewModel editBookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                book.Name = editBookViewModel.Name;
                book.Price = editBookViewModel.Price;
                book.Author = editBookViewModel.Author;
                book.ShortDescription = editBookViewModel.ShortDescription;
                book.Content = editBookViewModel.Content;
                book.CategoryId = editBookViewModel.SelectedCategory;
                book.Image = editBookViewModel.Image;
                await _context.SaveChangesAsync();

                toastNotification.AddSuccessToastMessage("A Book is updated successfully");
                return RedirectToAction("Index");
            }

            toastNotification.AddErrorToastMessage("An error occured when updating a Book");
            return View(editBookViewModel);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookProjectDbContext.Books'  is null.");
            }
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            toastNotification.AddSuccessToastMessage("A Book is deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
