using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class EmployeeProgressRepository : Repository<EmployeeProgress>
    {
        public EmployeeProgressRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
