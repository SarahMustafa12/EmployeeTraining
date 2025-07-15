using InternalTraining.Data;
using InternalTraining.Models;
using InternalTraining.Repositories;
using InternalTraining.Repositories.IRepository;

namespace InternalTraining.Unit_of_Work
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Chapter> Chapters { get; }
        IRepository<ContactUs> ContactsUs { get; }
        IRepository<Option> Options { get; }
        IRepository<Course> Courses { get; }
        IRepository<CourseFeedback> CourseFeedbacks { get; }
        IRepository<EmployeeProgress> EmployeesProgress { get; }
        IRepository<Exam> Exams { get; }
        IRepository<Lesson> Lessons { get; }
        IRepository<Payment> Payments { get; }
        IRepository<Question> Questions { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CompanyUser> CompanyUsers { get; }
        IRepository<EmployeeUser> EmployeeUsers { get; }

        IRepository<FirstPayment> FirstPayments { get; }
        IRepository<EmployeeCourse> EmployeeCourses { get; }
        IRepository<BookingCourse> BookingCourses { get; }

        IRepository<CourseInvitation> CourseInvitations { get; }

        IRepository<SecondPayment> SecondPayments  { get; }

        IRepository<CompanyContactUs> CompnanyContactsUs { get; }
        IRepository<ApplicationUser> ApplicationUsers { get; }

        IRepository<EmployeeLessonProgress> EmployeeLessonsProgress { get; }
        IRepository<EmployeeChapterExam> EmployeeChaptersExam { get; }


        void Commit();

    }
}
