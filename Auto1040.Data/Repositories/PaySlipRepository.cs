using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auto1040.Data.Repositories
{
    public class PaySlipRepository : Repository<PaySlip>, IPaySlipRepository
    {
        public PaySlipRepository(DataContext context) : base(context)
        {
        }
        public PaySlip? GetByIdWithUser(int id)
        {
            return _dbSet.Include(p=>p.User).FirstOrDefault(p=>p.Id==id);
        }

    }
}
