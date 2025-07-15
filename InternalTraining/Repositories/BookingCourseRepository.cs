using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class BookingCourseRepository : Repository<BookingCourse>
    {
        BookingCourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
