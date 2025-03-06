

using Leyadech.Core.Entities;
using Leyadech.Core.Repositories;

namespace Auto1040.Service
{
    class UserService(IRepository<User> userRepository)
    {
        private readonly IRepository<User> _userRepository = userRepository;

        public User? CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return _userRepository.Add(user);
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.Delete(id);
        }

        public User? UpdateUser(int id, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return _userRepository.Update(id, user);
        }

        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public IEnumerable<User> GetAllUsers()
        {

            return _userRepository.GetList();
        }
    }
}
