
using University_Application.Interfaces.Repositories;
using University_Domain.Entities;

namespace University_Application.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<StudentProfile> StudentProfiles { get; }

        IRepository<DoctorProfile> DoctorProfiles { get; }
        IRepository<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(
            CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default);
    }
}
