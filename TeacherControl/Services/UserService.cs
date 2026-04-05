using Microsoft.EntityFrameworkCore;
using TeacherControl.Data;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserResponseDto> Add(UserRequestDto userRequestDto)
    {
        //VERIFICA SE EMAIL JÁ EXISTE, CASO EXISTA NÃO DEVE SER ADICIONADO O USÁRIO INFORMADO!
        var existEmail = await userRepository.GetUserByEmail(userRequestDto.Email.ToLower());
        if (existEmail != null)
            throw new Exception("Email already registered!!!");
        
        //Criptografar senha

        var user = UserMapper.ToEntity(userRequestDto);
        
        await userRepository.Add(user);

        return UserMapper.ToResponse(user);
    }

    public async Task<UserResponseDto> Update(Guid id,UserRequestDto userRequestDto)
    {
        var userFound = await userRepository.GetById(id);

        if (userFound == null)
            throw new Exception("User not found!");
        

        
        userFound.SetEmail(userRequestDto.Email);
        userFound.SetPassword(userRequestDto.Password);
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

    public async Task<List<UserResponseDto>> GetAll()
    {
        var listUsers = await userRepository.GetAll();

        return listUsers.Select(UserMapper.ToResponse).ToList();
    }

    public async Task<UserResponseDto?> GetUserByEmail(string email)
    {
        var userByEmail = await userRepository.GetUserByEmail(email);

        if(userByEmail == null)
            throw new Exception("User not found!");

        return UserMapper.ToResponse(userByEmail);
    }
}