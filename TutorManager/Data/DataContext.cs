using Microsoft.EntityFrameworkCore;
using TutorManager.Models;

namespace TutorManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 

        }
        public DbSet<UserModel> UsersTable { get; set; }
    }
}
