using Microsoft.AspNetCore.Identity;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Enums;
using TeacherControl.Extensions;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class UserService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher<User> passwordHasher) : BaseService<User, UserRequestDto, UserResponseDto>(userRepository), IUserService
    
{
    protected override UserResponseDto ToResponse(User entity)
    {
        return UserMapper.ToResponse(entity);
    }

    protected override User ToEntity(UserRequestDto resquestDto)
    {
        return UserMapper.ToEntity(resquestDto);
    }

    public async Task<UserResponseDto> Create(UserRequestDto userRequestDto)
    {
        if (await userRepository.GetUserByEmail(userRequestDto.Email.ToLower()) != null)
            throw new DomainException("Email already registered");

        var role = await roleRepository.GetById(userRequestDto.RoleId);
        if (role == null)
            throw new DomainException("Role not found");
        
        var user = UserMapper.ToEntity(userRequestDto);
        
        user.Validate();
        
        var hashedPassword = passwordHasher.HashPassword(user, userRequestDto.Password);
        user.SetPassword(hashedPassword);

        await userRepository.Add(user);

        var userCreated = await userRepository.GetById(user.Id);

        return UserMapper.ToResponse(userCreated);
    }

    public async Task<UserResponseDto> Update(Guid id, UserRequestDto userRequestDto)
    {
        var userFound = await userRepository.GetById(id);

        if (userFound == null)
            throw new DomainException("User not found");

        var emailExists = await userRepository.GetUserByEmail(userRequestDto.Email);

        if (emailExists != null && emailExists.Id != id)
            throw new DomainException("Email already registered");

        var roleExists = await roleRepository.GetById(userRequestDto.RoleId);

        if (roleExists == null)
            throw new DomainException("Role not found");
        
        var hashed = passwordHasher.HashPassword(userFound, userRequestDto.Password);

        userFound.SetEmail(userRequestDto.Email);
        userFound.SetPassword(hashed);
        userFound.SetName(userRequestDto.Name);
        userFound.SetRoleId(userRequestDto.RoleId);
        
        await userRepository.Update(userFound);

        return UserMapper.ToResponse(userFound);
    }

    public async Task Remove(Guid id)
    {
        await userRepository.Remove(id);
    }

    public async Task<UserResponseDto> GetById(Guid id)
    {
        var userResponse = await userRepository.GetById(id);

        if (userResponse == null)
            throw new Exception("User not found!");

        return UserMapper.ToResponse(userResponse);
    }
    
    public async Task<List<UserResponseSummaryDto>> GetAll()
    {
        var listUsers = await userRepository.GetAll();

        return listUsers.Select(UserMapper.ToSumaryResponse).ToList();
    }
    
    public async Task<List<UserResponseDto>> GetAllFull()
    {
        var listUsers = await userRepository.GetAllFull();

        return listUsers.Select(UserMapper.ToResponse).ToList();
    }

    public async Task<UserResponseDto?> GetUserByEmail(string email)
    {
        var userByEmail = await userRepository.GetUserByEmail(email);

        if (userByEmail == null)
            throw new Exception("User not found!");

        return UserMapper.ToResponse(userByEmail);
    }
}