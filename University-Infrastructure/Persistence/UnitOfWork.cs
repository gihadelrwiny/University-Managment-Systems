
using CampusServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using University_Application.Interfaces;
using University_Application.Interfaces.Repositories;
using University_Domain.Entities;
using University_Infrastructure.Persistence;

namespace University_Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IDbContextTransaction? _currentTransaction;

        // Backing fields
        private IRepository<ApplicationUser>? _applicationUsers;
        private IRepository<StudentProfile>? _studentProfiles;
        private IRepository<RefreshToken>? _refreshTokens;
        private IRepository<DoctorProfile>? _doctorProfiles;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<ApplicationUser> Users =>
            _applicationUsers ??= new Repository<ApplicationUser>(_context);

        public IRepository<StudentProfile> StudentProfiles =>
            _studentProfiles ??= new Repository<StudentProfile>(_context);

        public IRepository<DoctorProfile> DoctorProfiles =>
            _doctorProfiles ??= new Repository<DoctorProfile>(_context);
        public IRepository<RefreshToken> RefreshTokens =>
       _refreshTokens ??= new Repository<RefreshToken>(_context);

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task BeginTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is not null)
            {
                throw new InvalidOperationException(
                    "A transaction is already in progress.");
            }

            _currentTransaction =
                await _context.Database.BeginTransactionAsync(
                    cancellationToken);
        }


        public async Task CommitTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null)
            {
                throw new InvalidOperationException(
                    "No transaction is in progress.");
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);

                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }


        public async Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);

            await _currentTransaction.DisposeAsync();

            _currentTransaction = null;
        }


        public void Dispose()
        {
            _currentTransaction?.Dispose();

            _context.Dispose();

            GC.SuppressFinalize(this);
        }


        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }

            await _context.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }
}
