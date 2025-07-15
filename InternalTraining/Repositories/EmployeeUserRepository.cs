using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class EmployeeUserRepository : Repository<EmployeeUser>
    {
        public EmployeeUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
