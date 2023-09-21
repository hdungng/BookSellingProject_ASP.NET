using BookSellingProject.Models.Domain;

namespace BookSellingProject.Models.ViewModels
{
    public class CreateCheckoutViewModel
    {
        public List<CartViewModel> CartItem { get; set; }

        public List<Book> Book { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string? Note { get; set; }

        public int Total { get; set; }
    }
}