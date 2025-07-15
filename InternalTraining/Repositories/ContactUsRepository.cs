using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class ContactUsRepository : Repository<ContactUs>
    {
        public ContactUsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
