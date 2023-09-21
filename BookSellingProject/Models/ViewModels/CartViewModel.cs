using BookSellingProject.Models.Domain;

namespace BookSellingProject.Models.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }

        public Book Book { get; set; }

        public int Quantity { get; set; }

        public int SubTotal { get; set; }

    }
}
