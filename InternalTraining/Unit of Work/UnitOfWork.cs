using InternalTraining.Data;
using InternalTraining.Models;
using InternalTraining.Repositories;
using InternalTraining.Repositories.IRepository;

namespace InternalTraining.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork

    {
        private readonly ApplicationDbContext _context;
        public IRepository<Chapter> Chapters { get; private set; }

        public IRepository<ContactUs> ContactsUs { get; private set; }

        public IRepository<Course> Courses { get; private set; }

        public IRepository<CourseFeedback> CourseFeedbacks { get; private set; }

        public IRepository<EmployeeProgress> EmployeesProgress { get; private set; }

        public IRepository<Exam> Exams { get; private set; }

        public IRepository<Lesson> Lessons { get; private set; }

        public IRepository<Payment> Payments { get; private set; }

        public IRepository<Question> Questions { get; private set; }
        public IRepository<Option> Options { get; private set; }
        public IRepository<Cart> Carts { get; private set; }
        public IRepository<CompanyUser> CompanyUsers { get; private set; }
        public IRepository<EmployeeUser> EmployeeUsers { get; private set; }


        public IRepository<FirstPayment> FirstPayments { get; private set; }
        public IRepository<BookingCourse> BookingCourses { get; private set; }
        public IRepository<EmployeeCourse> EmployeeCourses { get; private set; }

        public IRepository<CourseInvitation> CourseInvitations { get; private set; }
        public IRepository<SecondPayment> SecondPayments { get; private set; }

        public IRepository<CompanyContactUs> CompnanyContactsUs { get; private set; }

        public IRepository<ApplicationUser> ApplicationUsers { get; private set; }

<<<<<<< HEAD
        public IRepository<EmployeeLessonProgress> EmployeeLessonsProgress { get; private set; }
        public IRepository<EmployeeChapterExam> EmployeeChaptersExam { get; private set; }

=======
>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5

        public UnitOfWork(ApplicationDbContext _context)
        {
            this._context = _context;
            Chapters = new Repository<Chapter>(_context);
            ContactsUs = new Repository<ContactUs>(_context);
            Courses = new Repository<Course>(_context);
            CourseFeedbacks = new Repository<CourseFeedback>(_context);
            EmployeesProgress = new Repository<EmployeeProgress>(_context);
            Exams = new Repository<Exam>(_context);
            Lessons = new Repository<Lesson>(_context);
            Payments = new Repository<Payment>(_context);
            Questions = new Repository<Question>(_context);
            Options = new Repository<Option>(_context);
            Carts = new Repository<Cart>(_context);
            FirstPayments = new Repository<FirstPayment>(_context);
            BookingCourses = new Repository<BookingCourse>(_context);
            CompanyUsers = new Repository<CompanyUser>(_context);
            EmployeeUsers = new Repository<EmployeeUser>(_context);
            EmployeeCourses = new Repository<EmployeeCourse>(_context);
            CourseInvitations = new Repository<CourseInvitation>(_context);
            SecondPayments = new Repository<SecondPayment>(_context);
            CompnanyContactsUs = new Repository<CompanyContactUs>(_context);
            ApplicationUsers = new Repository<ApplicationUser>(_context);
<<<<<<< HEAD
            EmployeeLessonsProgress = new Repository<EmployeeLessonProgress>(_context);
            EmployeeChaptersExam = new Repository<EmployeeChapterExam>(_context);
=======

>>>>>>> cc9158b4a66e575c1b08ffb5cae51454e7b951c5
        }
        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose(); 
        }
    }
}
