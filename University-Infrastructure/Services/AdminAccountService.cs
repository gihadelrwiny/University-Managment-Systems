using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using University_Application.DTO;
using University_Application.Interfaces;
using University_Application.Interfaces.Iservice;
using University_Domain.Common;
using University_Domain.Entities;
using University_Domain.Enums;

namespace University_Infrastructure.Services
{
    public sealed class AdminAccountService(
        IUnitOfWork unitOfWork,
        IPasswordHasher<ApplicationUser> passwordHasher)
        : IAdminAccountService
    {
        public async Task<Result<string>> CreateStudentAsync(
            CreateStudentRequest request,
            CancellationToken ct)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            bool emailExists = await unitOfWork.Users.ExistsAsync(
                u => u.Email == email, ct);

            if (emailExists)
            {
                return Result<string>.Failure("Email already exists.");
            }

            bool codeExists = await unitOfWork.StudentProfiles.ExistsAsync(
                s => s.StudentCode == request.StudentCode, ct);

            if (codeExists)
            {
                return Result<string>.Failure("Student code already exists.");
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName.Trim(),
                Email = email,
                Role = UserRole.Student,
                IsActive = true
            };

            user.PasswordHash = passwordHasher.HashPassword(
                user, request.Password);

            try
            {
                await unitOfWork.BeginTransactionAsync(ct);

                await unitOfWork.Users.AddAsync(user, ct);

                await unitOfWork.SaveChangesAsync(ct);

                var profile = new StudentProfile
                {
                    UserId = user.Id,
                    StudentCode = request.StudentCode.Trim(),
                    Department = request.Department.Trim(),
                    AcademicYear = request.AcademicYear
                };

                await unitOfWork.StudentProfiles.AddAsync(profile, ct);

                await unitOfWork.CommitTransactionAsync(ct);

                return Result<string>.Success(
                    "Student account created successfully.");
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync(ct);

                return Result<string>.Failure(
                    "Failed to create student account.");
            }
        }

        public async Task<Result<string>> CreateDoctorAsync(
            CreateDoctorRequest request,
            CancellationToken ct)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            bool emailExists = await unitOfWork.Users.ExistsAsync(
                u => u.Email == email, ct);

            if (emailExists)
            {
                return Result<string>.Failure("Email already exists.");
            }

            bool codeExists = await unitOfWork.DoctorProfiles.ExistsAsync(
                d => d.EmployeeCode == request.EmployeeCode, ct);

            if (codeExists)
            {
                return Result<string>.Failure("Employee code already exists.");
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName.Trim(),
                Email = email,
                Role = UserRole.Doctor,
                IsActive = true
            };

            user.PasswordHash = passwordHasher.HashPassword(
                user, request.Password);

            try
            {
                await unitOfWork.BeginTransactionAsync(ct);

                await unitOfWork.Users.AddAsync(user, ct);

                await unitOfWork.SaveChangesAsync(ct);

                var profile = new DoctorProfile
                {
                    UserId = user.Id,
                    EmployeeCode = request.EmployeeCode.Trim(),
                    Department = request.Department.Trim(),
                    AcademicTitle = request.AcademicTitle.Trim()
                };

                await unitOfWork.DoctorProfiles.AddAsync(profile, ct);

                await unitOfWork.CommitTransactionAsync(ct);

                return Result<string>.Success(
                    "Doctor account created successfully.");
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync(ct);

                return Result<string>.Failure(
                    "Failed to create doctor account.");
            }
        }
    }
}

