using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class CourseFeedbackRepository : Repository<CourseFeedback>
    {
        public CourseFeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
