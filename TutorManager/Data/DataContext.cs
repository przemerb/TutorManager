using Microsoft.EntityFrameworkCore;
using TutorManager.Models;

namespace TutorManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<TutorModel> TutorTable { get; set; }   
        public DbSet<StudentModel> StudentTable { get; set; }
        public DbSet<LessonModel> LessonTable { get; set; }
    }
}
