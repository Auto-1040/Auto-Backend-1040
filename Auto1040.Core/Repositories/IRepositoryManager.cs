using Auto1040.Core.Repositories;

public interface IRepositoryManager
{
    IUserRepository Users { get; }
    IUserDetailsRepository UserDetails { get; }
    IPaySlipRepository PaySlips { get; }
    void Save();
}
