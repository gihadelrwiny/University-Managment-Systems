using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Text;
using University_Application.DTO;
using University_Application.Interfaces;
using University_Application.Interfaces.Iservice;
using University_Domain.Common;
using University_Domain.Entities;
using University_Infrastructure.Options;

namespace University_Infrastructure.Services
{

    public sealed class AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher<ApplicationUser> passwordHasher,
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions) : IAuthService
    {
        private readonly JwtOptions _options = jwtOptions.Value;

        public async Task<Result<AuthResponse>> LoginAsync(
            LoginRequest request,
            CancellationToken ct)
        {
            string email = request.Email
                .Trim()
                .ToLowerInvariant();

            var matches = await unitOfWork.Users.FindAsync(
                u => u.Email == email,
                cancellationToken: ct);

            ApplicationUser? user = matches.SingleOrDefault();

            if (user is null)
            {
                return Result<AuthResponse>.Failure(
                    "Invalid email or password.");
            }

            if (!user.IsActive)
            {
                return Result<AuthResponse>.Failure(
                    "Your account is inactive. Contact the administrator.");
            }

            var verification = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                return Result<AuthResponse>.Failure(
                    "Invalid email or password.");
            }

            if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = passwordHasher.HashPassword(
                    user,
                    request.Password);

                unitOfWork.Users.Update(user);
            }

            return await IssueTokensAsync(user, ct);
        }

        public async Task<Result<AuthResponse>> RefreshAsync(
            RefreshRequest request,
            CancellationToken ct)
        {
            var matches = await unitOfWork.RefreshTokens.FindAsync(
                rt => rt.Token == request.RefreshToken,
                includes: new Expression<Func<RefreshToken, object>>[]
                {
                rt => rt.User
                },
                cancellationToken: ct);

            RefreshToken? oldToken = matches.SingleOrDefault();

            if (oldToken is null)
            {
                return Result<AuthResponse>.Failure(
                    "Invalid refresh token.");
            }

            if (!oldToken.IsActive)
            {
                return Result<AuthResponse>.Failure(
                    "Refresh token is expired or revoked.");
            }

            if (!oldToken.User.IsActive)
            {
                return Result<AuthResponse>.Failure(
                    "Your account is inactive.");
            }

            // Revoke old token.
            oldToken.RevokedOnUtc = DateTime.UtcNow;

            unitOfWork.RefreshTokens.Update(oldToken);

            // Generate a new pair of tokens.
            return await IssueTokensAsync(oldToken.User, ct);
        }

        private async Task<Result<AuthResponse>> IssueTokensAsync(
            ApplicationUser user,
            CancellationToken ct)
        {
            string accessToken = tokenService.GenerateAccessToken(user);

            RefreshToken refreshToken =
                tokenService.GenerateRefreshToken(user.Id);

            await unitOfWork.RefreshTokens.AddAsync(
                refreshToken, ct);

            await unitOfWork.SaveChangesAsync(ct);

            DateTime accessTokenExpiresOnUtc =
                DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

            return Result<AuthResponse>.Success(
                new AuthResponse(
                    accessToken,
                    refreshToken.Token,
                    accessTokenExpiresOnUtc));
        }
    }
}
