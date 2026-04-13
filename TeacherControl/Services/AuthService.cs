using Microsoft.AspNetCore.Identity;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class AuthService (IUserRepository userRepository, IPasswordHasher<User> passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await userRepository.GetUserByEmail(loginRequestDto.Email);

        if (user == null)
            return null;

        var passwordValidation = passwordHasher.VerifyHashedPassword(user, user.Password, loginRequestDto.Password);

        if (passwordValidation == PasswordVerificationResult.Failed)
            return null;
        
        var token = tokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Email = user.Email,
            Name = user.Name,
            role = user.Role == null ? null : new RoleResponseDto
            {
                Name = user.Role.Name,
                Description = user.Role.Description
            },
            Token = token.ToString() ?? ""
        };

    }
}