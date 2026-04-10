using Microsoft.AspNetCore.Identity;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Mapper;
using TeacherControl.Models;
using TeacherControl.Repositories.Interfaces;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Services;

public class UserService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher<User> passwordHasher,
    TokenService tokenService) : BaseService<User, UserRequestDto, UserResponseDto>(userRepository), IUserService
    
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
        //VERIFICA SE EMAIL JÁ EXISTE, CASO EXISTA NÃO DEVE SER ADICIONADO O USÁRIO INFORMADO!
        var emailExist = await userRepository.GetUserByEmail(userRequestDto.Email.ToLower());
        if (emailExist != null)
            throw new Exception("Email already registered!!!");

        //VERIFICA SE O ROLE EXISTE, CASO NÃO EXISTA NÃO É POSSIVEL ADICIONAR O USER
        var roleExists = await roleRepository.GetById(userRequestDto.RoleId);
        if (roleExists == null)
            throw new Exception("Role not found!");

        var user = UserMapper.ToEntity(userRequestDto);

        // FAZ O HASHER DA SENHA
        var hashedPassword = passwordHasher.HashPassword(user, userRequestDto.Password);
        user.SetPassword(hashedPassword);

        await userRepository.Add(user);

        //Está retornando o objeto user que acabou de ser criado (SEM O ROLE) POR ISSO REALIZO O GETBYID.
        var userCreated = await userRepository.GetById(user.Id);

        return UserMapper.ToResponse(userCreated);
    }

    public async Task<UserResponseDto> Update(Guid id, UserRequestDto userRequestDto)
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
    
    
    //AJUSTE TEMP - NÃO COMMITAR ATÉ TERMINAR: 
    public async Task<List<UserResponseDto>> GetAll()
    {
        var listUsers = await userRepository.GetAll();

        return listUsers.Select(UserMapper.ToResponse).ToList();
    }

    public async Task<UserResponseDto?> GetUserByEmail(string email)
    {
        var userByEmail = await userRepository.GetUserByEmail(email);

        if (userByEmail == null)
            throw new Exception("User not found!");

        return UserMapper.ToResponse(userByEmail);
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await userRepository.GetUserByEmail(loginRequestDto.Email);

        if (user == null)
            return null;

        var passwordValidation = passwordHasher.VerifyHashedPassword(user, user.Password, loginRequestDto.Password);

        if (passwordValidation == PasswordVerificationResult.Failed)
            return null;
        
        var token = tokenService.GenerationToken(user);

        return new LoginResponseDto
        {
            Email = user.Email,
            Name = user.Name,
            role = user.Role == null ? null : new RoleResponseDto
            {
                Name = user.Role.Name,
                Description = user.Role.Description
            },
            Token = token
        };

    }

}