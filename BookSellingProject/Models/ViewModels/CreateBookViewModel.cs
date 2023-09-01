using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSellingProject.Models.ViewModels
{
    public class CreateBookViewModel
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Author { get; set; }

        public int Price { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        // Display Categories
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // Collect Categories
        public Guid SelectedCategory { get; set; }
    }
}
