using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using Auto1040.Data.Repositories;
using Auto1040.Data;
using Microsoft.EntityFrameworkCore;

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
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefault(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public IEnumerable<Role> GetUserRoles(int userId)
    {
        return _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToList();
    }
}
