using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Entities;

namespace University_Application.Interfaces.Iservice
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user);

        RefreshToken GenerateRefreshToken(int userId);
    }
}
