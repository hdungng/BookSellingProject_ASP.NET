using BookSellingProject.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookSellingProject.Data
{
    public class BookProjectDbContext : DbContext
    {
        public BookProjectDbContext(DbContextOptions<BookProjectDbContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }


        public DbSet<CartItem> CartItems { get; set; }
    }
}
