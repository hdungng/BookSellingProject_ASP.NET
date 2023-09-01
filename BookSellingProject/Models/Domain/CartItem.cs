using Microsoft.AspNetCore.Identity;

namespace BookSellingProject.Models.Domain
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Book Book { get; set; }

        public int Quantity  { get; set; }

        public int SubTotal { get; set; }

        public IdentityUser User { get; set; }
    }
}
