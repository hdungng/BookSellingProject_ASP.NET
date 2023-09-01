using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSellingProject.Data;
using BookSellingProject.Models.Domain;
using NToastNotify;

namespace BookSellingProject.Controllers
{
    public class AppRolesController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly IToastNotification toastNotification;

        public AppRolesController(AuthDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            this.toastNotification = toastNotification;
        }

        // GET: AppRoles
        public async Task<IActionResult> Index()
        {
            return _context.ApplicationRole != null ?
                        View(await _context.ApplicationRole.ToListAsync()) :
                        Problem("Entity set 'AuthDbContext.ApplicationRole'  is null.");
        }

        // GET: AppRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ApplicationRole == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.ApplicationRole
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // GET: AppRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Name")] ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                applicationRole.Id = Guid.NewGuid().ToString();
                applicationRole.NormalizedName = applicationRole.Name;
                _context.Add(applicationRole);
                await _context.SaveChangesAsync();
                toastNotification.AddSuccessToastMessage("A Role is created successfully");
                return RedirectToAction(nameof(Index));
            }
            return View(applicationRole);
        }

        // GET: AppRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ApplicationRole == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.ApplicationRole.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }
            return View(applicationRole);
        }

        // POST: AppRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Description,Name,Id")] ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    applicationRole.NormalizedName = applicationRole.Name;
                    _context.Update(applicationRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationRoleExists(applicationRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                toastNotification.AddSuccessToastMessage("A Role is updated successfully");
                return RedirectToAction(nameof(Index));
            }
            toastNotification.AddErrorToastMessage("An error occured when updating a Role");
            return View(applicationRole);
        }

        // GET: AppRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ApplicationRole == null)
            {
                return NotFound();
            }

            var applicationRole = await _context.ApplicationRole
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // POST: AppRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ApplicationRole == null)
            {
                return Problem("Entity set 'AuthDbContext.ApplicationRole'  is null.");
            }
            var applicationRole = await _context.ApplicationRole.FindAsync(id);
            if (applicationRole != null)
            {
                _context.ApplicationRole.Remove(applicationRole);
            }

            await _context.SaveChangesAsync();
            toastNotification.AddSuccessToastMessage("A Role is deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationRoleExists(string id)
        {
            return (_context.ApplicationRole?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
