using Microsoft.AspNetCore.Identity;

namespace BookSellingProject.Models.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public int Quantity  { get; set; }

        public int SubTotal { get; set; }

        public Invoice Invoices { get; set; }
    }
}
