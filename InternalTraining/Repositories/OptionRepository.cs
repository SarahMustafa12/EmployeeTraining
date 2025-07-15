using InternalTraining.Repositories;
using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class OptionsRepository : Repository<Option>
    {
        public OptionsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
