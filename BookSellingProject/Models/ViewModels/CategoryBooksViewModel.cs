using BookSellingProject.Models.Domain;

namespace BookSellingProject.Models.ViewModels
{
    public class CategoryBooksViewModel
    {
        public int? BooksQuantity { get; set; }

        public string? CurrentCategoryName { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();

        public List<Book> Books { get; set; } = new List<Book>();

    }
}
