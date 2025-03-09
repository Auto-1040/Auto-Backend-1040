using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;

namespace Auto1040.Data.Repositories
{
    public class OutputFormRepository : Repository<OutputForm>, IOutputFormRepository
    {
        public OutputFormRepository(DataContext context) : base(context)
        {
        }

        // Implement any additional methods specific to OutputForm if needed
    }
}
