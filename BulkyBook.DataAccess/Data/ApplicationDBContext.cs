
using Microsoft.EntityFrameworkCore;
using BulkyBook.Models;

namespace BulkyBook.DataAccess
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
    }

}
