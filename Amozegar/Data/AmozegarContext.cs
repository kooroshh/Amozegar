using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data
{
    public class AmozegarContext : IdentityDbContext<User, UserRole, string>
    {
        public AmozegarContext(DbContextOptions<AmozegarContext> option) : base(option) { }

        public DbSet<Report> Reports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<ClassRoam> Classes { get; set; }
        public DbSet<ClassStudents> ClassesStudents { get; set; }
        public DbSet<ClassStudentState> ClassesStudentsStates { get; set; }
        public DbSet<ClassStates> ClassesStates { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureType> PictureTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Report>()
                .Property(r => r.Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ClassRoam>()
                .Property(c => c.Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Notification>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .Property(u => u.PicturePath)
                .HasDefaultValue("user.webp");

            modelBuilder.Entity<ClassRoam>()
                .Property(c => c.ClassImage)
                .HasDefaultValue("classes.png");


            modelBuilder.Entity<ClassStudents>()
                .HasOne(stc => stc.User)
                .WithMany(u => u.StudentToClasses)
                .HasForeignKey(stc => stc.StudentId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ClassStudents>()
                .HasOne(stc => stc.Class)
                .WithMany(c => c.StudentToClasses)
                .HasForeignKey(stc => stc.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassRoam>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); 

        }

    }
}
