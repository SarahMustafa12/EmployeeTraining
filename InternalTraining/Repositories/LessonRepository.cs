using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class LessonRepository : Repository<Lesson>
    {
        public LessonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
