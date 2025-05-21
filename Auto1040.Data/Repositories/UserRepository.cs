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

    public User? GetUserWithRoles(string usernameOrEmail)
    {
        var user = _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);

        return user;
    }

    public IEnumerable<User> GetAllUsersWithRoles()
    {
        return _context.Users.Include(u => u.Roles);
    }
    public IEnumerable<Role> GetUserRoles(int userId)
    {
        var user = _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Id == userId);

        return user?.Roles ?? Enumerable.Empty<Role>();
    }
    public User? UpdateUserWithRoles(int id,User user)
    {
        var existingUser = _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return null;

        // Update user properties
        existingUser.UserName = user.UserName ?? existingUser.UserName;
        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.HashedPassword = user.HashedPassword ?? existingUser.HashedPassword;
        existingUser.UpdatedAt = DateTime.UtcNow;

        // Update roles
        existingUser.Roles.Clear();
        if (user.Roles != null)
        {
            foreach (var role in user.Roles)
            {
                var existingRole = _context.Roles.Find(role.Id);
                if (existingRole != null)
                {
                    existingUser.Roles.Add(existingRole);
                }
            }
        }
        return existingUser;
    }
    
}
