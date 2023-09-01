using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookSellingProject.Models.ViewModels
{
    public class EditBookViewModel
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Author { get; set; }

        public int Price { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        // Display Category
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // Collect Category
        public Guid SelectedCategory { get; set; }
    }
}