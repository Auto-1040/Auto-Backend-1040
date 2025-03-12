using Auto1040.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core.Repositories
{
    public interface IUserDetailsRepository:IRepository<UserDetails>
    {
        UserDetails? GetByUserId(int id);
        UserDetails? UpdateByUserId(int userId,UserDetails userDetails);
        bool DeleteByUserId(int userId);
    }
}
