using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class ExamRepository : Repository<Exam>
    {
        public ExamRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
