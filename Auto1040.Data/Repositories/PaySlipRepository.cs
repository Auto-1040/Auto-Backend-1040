using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;

namespace Auto1040.Data.Repositories
{
    public class PaySlipRepository : Repository<PaySlip>, IPaySlipRepository
    {
        public PaySlipRepository(DataContext context) : base(context)
        {
        }

        // Implement any additional methods specific to PaySlip if needed
    }
}
