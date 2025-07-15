using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    
    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
