using System.Linq.Expressions;
using UserManagementApp.UserApi.Models.Dtos;

namespace UserManagementApp.UserApi.Repositories.IRepositories
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<AppUser>>
            GetUsersAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true, int pageSize = 0, int pageNumber = 1);
        Task<AppUser> 
            GetUserAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true);
        Task CreateUserAsync(AppUser user);
        Task RemoveUserAsync(AppUser user);
        Task UpdateUserAsync(AppUser user);
        Task SaveAsync();
    }
}
