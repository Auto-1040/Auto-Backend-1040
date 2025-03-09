using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Data.Repositories
{
    public class RepositoryManager(DataContext context,IUserRepository userRepository,IUserDetailsRepository userDetailsRepository
        ,IPaySlipRepository paySlipRepository,IOutputFormRepository outputFormRepository):IRepositoryManager
    {
        private readonly DataContext _context=context;
        public IUserRepository Users { get; }= userRepository;
        public IUserDetailsRepository UserDetails { get; }=userDetailsRepository;
        public IPaySlipRepository PaySlips { get; } = paySlipRepository;
        public IOutputFormRepository OutputForms { get; } = outputFormRepository;
        public void Save()
        {
            _context.SaveChanges();
        }

        
    }
}
