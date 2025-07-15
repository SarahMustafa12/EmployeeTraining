using InternalTraining.Data;
using InternalTraining.Models;

namespace InternalTraining.Repositories
{
    public class PaymentRepository : Repository<Payment>
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
