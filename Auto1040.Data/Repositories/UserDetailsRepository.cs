using Auto1040.Core.Entities;
using Auto1040.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto1040.Data.Repositories
{
    public class UserDetailsRepository:Repository<UserDetails>,IUserDetailsRepository
    {
        private readonly DataContext _context;
        public UserDetailsRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }

        public bool DeleteByUserId(int userId)
        {
            var userDetails=_context.UserDetails.FirstOrDefault(u => u.UserId == userId);
            if(userDetails==null) 
                return false;
            _context.UserDetails.Remove(userDetails);
            return true;
        }

        public UserDetails? GetByUserId(int userId)
        {
            return _context.UserDetails.FirstOrDefault(u => u.UserId == userId);
        }

        public UserDetails? UpdateByUserId(int userId, UserDetails userDetails)
        {
            var source = _context.UserDetails.FirstOrDefault(u => u.UserId == userId);
            if (source == null)
                return null;
            UpdateAllProperties(source, userDetails);
            return source;
        }

        private void UpdateAllProperties(UserDetails source, UserDetails userDetails)
        {
            if (userDetails.FirstName != null)
                source.FirstName = userDetails.FirstName;
            if (userDetails.LastName != null)
                source.LastName = userDetails.LastName;
            if (userDetails.Ssn != null)
                source.Ssn = userDetails.Ssn;
            if (userDetails.SpouseFirstName != null)
                source.SpouseFirstName = userDetails.SpouseFirstName;
            if (userDetails.SpouseLastName != null)
                source.SpouseLastName = userDetails.SpouseLastName;
            if (userDetails.SpouseSsn != null)
                source.SpouseSsn = userDetails.SpouseSsn;
            if (userDetails.HomeAddress != null)
                source.HomeAddress = userDetails.HomeAddress;
            if (userDetails.City != null)
                source.City = userDetails.City;
            if (userDetails.State != null)
                source.State = userDetails.State;
            if (userDetails.ZipCode != null)
                source.ZipCode = userDetails.ZipCode;
            if (userDetails.ForeignCountry != null)
                source.ForeignCountry = userDetails.ForeignCountry;
            if (userDetails.ForeignState != null)
                source.ForeignState = userDetails.ForeignState;
            if (userDetails.ForeignPostalCode != null)
                source.ForeignPostalCode = userDetails.ForeignPostalCode;
            if (userDetails.FilingStatus != null)
                source.FilingStatus = userDetails.FilingStatus;
            if (userDetails.Dependents != null)
                source.Dependents = userDetails.Dependents;
        }
    }
}
