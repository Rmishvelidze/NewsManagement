using NewsManagement.Domain.Models.UserModels;

namespace NewsManagement.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public User? Get(UserLogin userLogin);
    }
}
