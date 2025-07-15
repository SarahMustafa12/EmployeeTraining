using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class CompanyCourseRepository : Repository<CompanyCourse>
    {
        CompanyCourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
