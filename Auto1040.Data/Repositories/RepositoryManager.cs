﻿using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Data.Repositories
{
    public class RepositoryManager(DataContext context,IUserRepository userRepository,IUserDetailsRepository userDetailsRepository):IRepositoryManager
    {
        private readonly DataContext _context=context;
        public IUserRepository Users { get; }= userRepository;
        public IUserDetailsRepository UserDetails { get; }=userDetailsRepository;
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
