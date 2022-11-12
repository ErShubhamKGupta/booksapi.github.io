using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Models
{
    public class BooksAPIDBContext : DbContext
    {
        public BooksAPIDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Books> tbl_Books { get; set; }
    }
}
