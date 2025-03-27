using Auto1040.Core.Entities;

namespace Auto1040.Core.Repositories
{
    public interface IPaySlipRepository : IRepository<PaySlip>
    {
        public PaySlip? GetByIdWithUser(int id);
    }

}
