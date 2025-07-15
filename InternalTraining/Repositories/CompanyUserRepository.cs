using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    
    public class CompanyUserRepository : Repository<CompanyUser>
    {
        public CompanyUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
