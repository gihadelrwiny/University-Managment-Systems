using System;
using System.Collections.Generic;
using System.Text;
using University_Application.DTO;
using University_Domain.Common;

namespace University_Application.Interfaces.Iservice
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(
     LoginRequest request,
     CancellationToken ct);

        Task<Result<AuthResponse>> RefreshAsync(
            RefreshRequest request,
            CancellationToken ct);
    }
}
