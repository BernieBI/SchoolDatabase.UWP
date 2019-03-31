using System.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

using SchoolDatabase.UWP.Model;

namespace SchoolDatabase.UWP.DataAccess
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "donau.hiof.no",
                InitialCatalog = "matslb",
                UserID = "matslb",
                Password = "q6mVnfLJ"
            };
            optionsBuilder.UseSqlServer(connStringBuilder.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
        }
    }
}
