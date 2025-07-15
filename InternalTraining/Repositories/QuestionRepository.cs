using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class QuestionRepository : Repository<Question>
    {
        public QuestionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
