using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using University_Application.Interfaces.Iservice;
using University_Application.DTO;

namespace University_Managment_System.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginRequest request,
            CancellationToken ct)
        {
            var result = await authService.LoginAsync(request, ct);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            RefreshRequest request,
            CancellationToken ct)
        {
            var result = await authService.RefreshAsync(request, ct);

            if (!result.IsSuccess)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}