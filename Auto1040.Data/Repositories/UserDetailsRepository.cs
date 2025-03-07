using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Data.Repositories
{
    public class UserDetailsRepository:Repository<UserDetails>,IUserDetailsRepository
    {
        public UserDetailsRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
}
