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
        public DbSet<TableType> TableTypes { get; set; }
        public DbSet<UserView> UsersViews { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkState> HomeworksStates { get; set; }
        public DbSet<ClassStudentsToHomework> ClassStudentsToHomeworks { get; set; }
        public DbSet<ClassStudentsToHomeworkState> ClassStudentsToHomeworkStates { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamState> ExamStates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<ClassStudentsToExam> ClassStudentsToExam { get; set; }
        public DbSet<ClassStudentsToExamToQuestion> ClassStudentsToExamsToQuestions { get; set; }


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


            modelBuilder.Entity<Homework>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<ClassStudentsToHomework>()
                .Property(c => c.SendAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Exam>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<ClassStudentsToExam>()
                .Property(c => c.JoinAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<ClassStudentsToExamToQuestion>()
                .Property(c => c.CompletedAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<Question>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<ClassStudents>()
                .Property(c => c.JoinAt)
                .HasDefaultValueSql("GETDATE()");



            modelBuilder.Entity<User>()
                .Property(u => u.PicturePath)
                .HasDefaultValue("user.webp");

            modelBuilder.Entity<ClassRoam>()
                .Property(c => c.ClassImage)
                .HasDefaultValue("classes.png");



            modelBuilder.Entity<ClassStudentsToExam>()
                .Property(c => c.IsFinish)
                .HasDefaultValue(false);

            modelBuilder.Entity<ClassStudentsToExam>()
                .Property(c => c.LastCompletedQuestion)
                .HasDefaultValue(0);




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




            modelBuilder.Entity<ClassStudentsToExamToQuestion>()
                .HasOne(csteq => csteq.Question)
                .WithMany(q => q.ClassStudentsToExamToQuestions)
                .HasForeignKey(csteq => csteq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ClassStudentsToExamToQuestion>()
                .HasOne(csteq => csteq.ClassStudentsToExam)
                .WithMany(cste => cste.ClassStudentsToExamToQuestions)
                .HasForeignKey(csteq => csteq.ClassStudentToExamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassStudentsToExamToQuestion>()
                .HasOne(csteq => csteq.SelectedOption)
                .WithMany(qo => qo.ClassStudentsToExamToQuestions)
                .HasForeignKey(csteq => csteq.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.ClassStudentsToExam)
                .WithOne(cste => cste.Exam)
                .HasForeignKey(cste => cste.ExamId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<QuestionOption>()
                .HasMany(qo => qo.ClassStudentsToExamToQuestions)
                .WithOne(cstetq => cstetq.SelectedOption)
                .HasForeignKey(cstetq => cstetq.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionOption>()
                .HasOne(qo => qo.Question)
                .WithMany(q => q.QuestionOptions)
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.QuestionOptions)
                .WithOne(qo => qo.Question)
                .HasForeignKey(qo => qo.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.ClassStudentsToExamToQuestions)
                .WithOne(cstetq => cstetq.Question)
                .HasForeignKey(cstetq => cstetq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);



        }


    }
}
