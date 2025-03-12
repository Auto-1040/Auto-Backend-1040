using Auto1040.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core.Repositories
{
    public interface IRoleRepository:IRepository<Role>
    {
        Role? GetByNmae(string roleName);
    }
}
