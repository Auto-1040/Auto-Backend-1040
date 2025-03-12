using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;

public interface IRepositoryManager
{
    public IUserRepository Users { get; }
    public IUserDetailsRepository UserDetails { get; }
    public IPaySlipRepository PaySlips { get; }
    public IOutputFormRepository OutputForms { get; }
    IRoleRepository Roles { get; }
    void Save();
}
