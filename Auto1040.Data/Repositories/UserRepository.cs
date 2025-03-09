using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Data.Repositories;
using Auto1040.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public User GetUserWithRoles(string usernameOrEmail)
    {
        return _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public IEnumerable<Role> GetUserRoles(int userId)
    {
        var user = _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Id == userId);

        return user?.Roles ?? Enumerable.Empty<Role>();
    }
}
