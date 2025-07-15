using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class ChapterRepository : Repository<Chapter>
    {
        public ChapterRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
