using Microsoft.AspNetCore.Identity;

namespace BookSellingProject.Models.ViewModels
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }


    }
}