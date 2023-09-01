using Microsoft.AspNetCore.Identity;

namespace BookSellingProject.Models.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
