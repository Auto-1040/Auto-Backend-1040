﻿using Auto1040.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<Role> GetUserRoles(int userId);
        IEnumerable<User> GetAllUsersWithRoles();

        User? GetUserWithRoles(string usernameOrEmail);

        User? UpdateUserWithRoles(int id, User user);



    }
}
