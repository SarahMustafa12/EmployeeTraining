using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
