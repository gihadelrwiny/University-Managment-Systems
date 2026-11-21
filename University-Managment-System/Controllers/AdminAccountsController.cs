using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using University_Application.DTO;
using University_Application.Interfaces.Iservice;

namespace University_Managment_System.Controllers
{
    [ApiController]
    [Route("api/admin/accounts")]
    [Authorize(Roles = "Admin")]
    public class AdminAccountsController(
     IAdminAccountService accountService) : ControllerBase
    {
        [HttpPost("students")]
        public async Task<IActionResult> CreateStudent(
            CreateStudentRequest request,
            CancellationToken ct)
        {
            var result = await accountService.CreateStudentAsync(
                request, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(
            CreateDoctorRequest request,
            CancellationToken ct)
        {
            var result = await accountService.CreateDoctorAsync(
                request, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}