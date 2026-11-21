using System;
using System.Collections.Generic;
using System.Text;
using University_Application.DTO;
using University_Domain.Common;


namespace University_Application.Interfaces.Iservice
{
    public interface IAdminAccountService
    {
        Task<Result<string>> CreateStudentAsync(
            CreateStudentRequest request,
            CancellationToken ct);

        Task<Result<string>> CreateDoctorAsync(
            CreateDoctorRequest request,
            CancellationToken ct);
    }
}
