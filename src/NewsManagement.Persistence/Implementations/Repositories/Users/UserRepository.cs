using NewsManagement.Application.Interfaces.Repositories;
using NewsManagement.Persistence.Data;
using NewsManagementMinimal.Models;

namespace NewsManagement.Persistence.Implementations.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        public User? Get(UserLogin userLogin) =>
            UserDataContext.Users.FirstOrDefault
            (o => o != null && o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)
                            && o.Password.Equals(userLogin.Password));
    }
}
