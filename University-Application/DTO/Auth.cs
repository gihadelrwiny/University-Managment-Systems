using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace University_Application.DTO
{

    public sealed record CreateStudentRequest(
       [Required, MaxLength(100)] string FullName,
       [Required, EmailAddress] string Email,
       [Required, MinLength(8)] string Password,
       [Required, MaxLength(50)] string StudentCode,
       [Required, MaxLength(100)] string Department,
       [Required, Range(1, 10)] int AcademicYear
   );

    public sealed record CreateDoctorRequest(
        [Required, MaxLength(100)] string FullName,
        [Required, EmailAddress] string Email,
        [Required, MinLength(8)] string Password,
        [Required, MaxLength(50)] string EmployeeCode,
        [Required, MaxLength(100)] string Department,
        [Required, MaxLength(100)] string AcademicTitle
    );

    public sealed record LoginRequest(
        [Required, EmailAddress] string Email,
        [Required] string Password
    );

    public sealed record RefreshRequest(
        [Required] string RefreshToken
    );

    public sealed record AuthResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresOnUtc
    );
}
