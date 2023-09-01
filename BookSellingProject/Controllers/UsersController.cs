using BookSellingProject.Data;
using BookSellingProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BookSellingProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IToastNotification toastNotification;
        private readonly AuthDbContext authDbContext;

        public UsersController(UserManager<IdentityUser> userManager,
            IToastNotification toastNotification, AuthDbContext authDbContext)
        {
            this.userManager = userManager;
            this.toastNotification = toastNotification;
            this.authDbContext = authDbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Get List<IdentityUser> Users
            var users = await authDbContext.Users.ToListAsync();

            var currentUser = await authDbContext.Users.
                FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            if (currentUser is not null)
            {
                users.Remove(currentUser);
            }

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            // Pass values to UserViewModel 
            foreach (var user in users)
            {
                usersViewModel.Users.Add(new User
                {
                    Id = Guid.Parse(user.Id),
                    Username = user.UserName,
                    EmailAddress = user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email
            };

            var identityResult = await userManager.CreateAsync(identityUser, userViewModel.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    // assign roles to the user
                    var roles = new List<string> { "User" };

                    if (userViewModel.AdminRoleCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        toastNotification.AddSuccessToastMessage("A User is created successfully");
                        return RedirectToAction("List", "Users");
                    }
                }
            }
            toastNotification.AddErrorToastMessage("An error occured when creating a User");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user is not null && user.UserName != User.Identity.Name)
            {
                var identityResult = await userManager.DeleteAsync(user);

                if (identityResult is not null && identityResult.Succeeded)
                {
                    toastNotification.AddSuccessToastMessage("A User is deleted successfully");
                    return RedirectToAction("List", "Users");
                }
            }

            toastNotification.AddErrorToastMessage("An error occured when deleting a User");
            return RedirectToAction("List", "Users");
        }
    }
}
