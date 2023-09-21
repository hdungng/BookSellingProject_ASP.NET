namespace BookSellingProject.Models.Domain
{
    public class Invoice
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Total { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
