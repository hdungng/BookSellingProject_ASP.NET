using BookSellingProject.Models.Domain;

namespace BookSellingProject.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<Book> Books { get; set; }

        public  string Keyword { get; set; }
    }
}