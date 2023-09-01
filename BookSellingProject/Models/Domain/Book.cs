namespace BookSellingProject.Models.Domain
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Author { get; set; }

        public int Price { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CategoryId { get; set; }

        // nav
        public Category Category { get; set; }
    }
}
