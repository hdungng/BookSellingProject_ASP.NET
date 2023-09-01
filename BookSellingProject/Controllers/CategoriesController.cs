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
using NToastNotify;
using Microsoft.AspNetCore.Authorization;

namespace BookSellingProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly BookProjectDbContext _context;
        private readonly IToastNotification toastNotification;

        public CategoriesController(BookProjectDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = createCategoryViewModel.Name,
                    CreatedAt = DateTime.Now,
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("A Category is added successfully");
                return RedirectToAction("Index");
            }

            toastNotification.AddErrorToastMessage("An error occured when adding a Category");
            return View(createCategoryViewModel);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var categoryDomain = await _context.Categories.FindAsync(id);
            if (categoryDomain == null)
            {
                return NotFound();
            }

            var categoryViewModel = new EditCategoryViewModel
            {
                Name = categoryDomain.Name,
            };

            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditCategoryViewModel editCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id); 
                
                if(category == null) { return NotFound(); }

                category.Name = editCategoryViewModel.Name;
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("A Category is added successfully");
                return RedirectToAction("Index");
            }
            toastNotification.AddErrorToastMessage("An error occured when updating a Category");
            return View(editCategoryViewModel);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'BookProjectDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("A Category is added successfully");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
