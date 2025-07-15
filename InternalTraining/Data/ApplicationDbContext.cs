using InternalTraining.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using InternalTraining.Models.ViewModel;

namespace InternalTraining.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Chapter> Chapters { get; }
        public DbSet<ContactUs> ContactsUs { get; }
        public DbSet<Option> Options { get; }
        public DbSet<Course> Courses { get; }
        public DbSet<CourseFeedback> CourseFeedbacks { get; }
        public DbSet<EmployeeProgress> EmployeesProgress { get; }
        public DbSet<Exam> Exams { get; }
        public DbSet<Lesson> Lessons { get; }
        public DbSet<Payment> Payments { get; }
        public DbSet<Question> Questions { get; }
        public DbSet<Cart> Carts { get; }
        public DbSet<FirstPayment> firstPayments { get; }
        public DbSet<BookingCourse> BookingCourses { get; }
        public DbSet<CompanyUser> CompanyUsers { get; }  
        public DbSet<EmployeeCourse> employeeCourses { get; }
        public DbSet<EmployeeUser> EmployeeUsers { get; }
        public DbSet<SecondPayment> SecondPayments  { get; }
        public DbSet<CourseInvitation> CourseInvitations  { get; }

        public DbSet<CompanyContactUs> CompanyContactsUs { get; }

        public DbSet<EmployeeLessonProgress> EmployeeLessonsProgress { get; }
        public DbSet<EmployeeChapterExam> EmployeeChaptersExam { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Lesson>()
                .HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EmployeeCourse>().HasKey(e => new {e.EmployeeUserId, e.CourseId});

            // EmployeeUser → ApplicationUser (1:1)
            builder.Entity<EmployeeUser>()
                .HasOne(e => e.ApplicationUser)
                .WithOne(a => a.EmployeeUser)
                .HasForeignKey<EmployeeUser>(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete path 1

            // CompanyUser → ApplicationUser (1:1)
            builder.Entity<CompanyUser>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(a => a.CompanyUser)
                .HasForeignKey<CompanyUser>(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete path 2

            // EmployeeUser → CompanyUser (many:1)
            builder.Entity<EmployeeUser>()
                .HasOne(e => e.CompanyUser)
                .WithMany(c => c.EmployeeUsers)
                .HasForeignKey(e => e.CompanyUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade from CompanyUser to EmployeeUser


          

        }


        //public DbSet<InternalTraining.Models.ViewModel.RegisterVm> RegisterVm { get; set; } = default!;
        //public DbSet<InternalTraining.Models.ViewModel.LoginVm> LoginVm { get; set; } = default!;
        //public DbSet<InternalTraining.Models.ViewModel.ForgetPasswordVm> ForgetPasswordVm { get; set; } = default!;
        //public DbSet<InternalTraining.Models.ViewModel.ResetPasswordVm> ResetPasswordVm { get; set; } = default!;
        //public DbSet<InternalTraining.Models.ViewModel.ProfileVm> ProfileVm { get; set; } = default!;
    }
}
