using System.Linq.Expressions;

namespace UserManagementApp.UserApi.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly UserContext _context;
        internal DbSet<AppUser> _userDbSet;
        private bool _disposed = false;

        public UserRepository(IDbContextFactory<UserContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
            _userDbSet = _context.Set<AppUser>();
        }

        public async Task CreateUserAsync(AppUser user)
        {
            await _userDbSet.AddAsync(user);
            await SaveAsync();
        }

        public async Task<AppUser> 
            GetUserAsync(Expression<Func<AppUser, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<AppUser> query = _userDbSet;

            if (!tracked)
                query = query.AsNoTracking();
            
            if (filter is not null)
                query = query.Where(filter);

            AppUser? appUser = await query.FirstOrDefaultAsync();

            return appUser!;             
        }

        public async Task<IEnumerable<AppUser>>
            GetUsersAsync(
                Expression<Func<AppUser, bool>>? filter = null, 
                bool tracked = true,
                int pageSize = 0, 
                int pageNumber = 1)
        {
            IQueryable<AppUser> query = _userDbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (pageSize > 0)
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            IEnumerable<AppUser> usersList = await query.ToListAsync();

            return usersList;
        }

        public async Task UpdateUserAsync(AppUser user)
        {
            _userDbSet.Update(user);
            await SaveAsync();
        }

        public async Task RemoveUserAsync(AppUser user)
        {
            _userDbSet.Remove(user);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
