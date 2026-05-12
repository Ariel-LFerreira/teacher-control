using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherControl.DTOs.Requests;
using TeacherControl.DTOs.Response;
using TeacherControl.Services.Interfaces;

namespace TeacherControl.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService): ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() =>
        Ok(new {
            Name = User.FindFirst(ClaimTypes.Name)?.Value,
            NameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = User.FindFirst(ClaimTypes.Email)?.Value,
            Role = User.FindFirst(ClaimTypes.Role )?.Value,
            GivenName = User.FindFirst(ClaimTypes.GivenName)?.Value,
            Surname = User.FindFirst(ClaimTypes.Surname)?.Value,
            MobilePhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value,
            HomePhone = User.FindFirst(ClaimTypes.HomePhone)?.Value,
            OtherPhone = User.FindFirst(ClaimTypes.OtherPhone)?.Value,
            StreetAddress = User.FindFirst(ClaimTypes.StreetAddress)?.Value,
            Locality = User.FindFirst(ClaimTypes.Locality)?.Value,
            StateOrProvince = User.FindFirst(ClaimTypes.StateOrProvince)?.Value,
            Country = User.FindFirst(ClaimTypes.Country)?.Value,
            PostalCode = User.FindFirst(ClaimTypes.PostalCode)?.Value,
            DateOfBirth = User.FindFirst(ClaimTypes.DateOfBirth)?.Value,
            Gender = User.FindFirst(ClaimTypes.Gender)?.Value,
            Uri = User.FindFirst(ClaimTypes.Uri)?.Value,
            Webpage = User.FindFirst(ClaimTypes.Webpage)?.Value,
            Sid = User.FindFirst(ClaimTypes.Sid)?.Value,
            PrimarySid = User.FindFirst(ClaimTypes.PrimarySid)?.Value,
            GroupSid = User.FindFirst(ClaimTypes.GroupSid)?.Value,
            PrimaryGroupSid = User.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value,
            Anonymous = User.FindFirst(ClaimTypes.Anonymous)?.Value,
            Authentication= User.FindFirst(ClaimTypes.Authentication)?.Value,
            AuthenticationInstant = User.FindFirst(ClaimTypes.AuthenticationInstant)?.Value,
            AuthenticationMethod = User.FindFirst(ClaimTypes.AuthenticationMethod)?.Value,
            Hash = User.FindFirst(ClaimTypes.Hash)?.Value,
            DenyOnlyPrimarySid = User.FindFirst(ClaimTypes.DenyOnlyPrimarySid)?.Value,
            DenyOnlySid = User.FindFirst(ClaimTypes.DenyOnlySid)?.Value,
            DenyOnlyPrimaryGroupSid = User.FindFirst(ClaimTypes.DenyOnlyPrimaryGroupSid)?.Value,
            DenyOnlyWindowsDeviceGroup = User.FindFirst(ClaimTypes.DenyOnlyWindowsDeviceGroup)?.Value,
            Dsa = User.FindFirst(ClaimTypes.Dsa)?.Value,
            Expiration = User.FindFirst(ClaimTypes.Expiration)?.Value,
            Expired = User.FindFirst(ClaimTypes.Expired)?.Value,
            IsPersistent = User.FindFirst(ClaimTypes.IsPersistent)?.Value,
            Rsa = User.FindFirst(ClaimTypes.Rsa)?.Value,
            SerialNumber = User.FindFirst(ClaimTypes.SerialNumber)?.Value,
            System = User.FindFirst(ClaimTypes.System)?.Value,
            Thumbprint = User.FindFirst(ClaimTypes.Thumbprint)?.Value,
            Upn = User.FindFirst(ClaimTypes.Upn)?.Value,
            UserData = User.FindFirst(ClaimTypes.UserData)?.Value,
            Version = User.FindFirst(ClaimTypes.Version)?.Value,
            WindowsAccountName = User.FindFirst(ClaimTypes.WindowsAccountName)?.Value,
            WindowsDeviceClaim = User.FindFirst(ClaimTypes.WindowsDeviceClaim)?.Value,
            WindowsDeviceGroup = User.FindFirst(ClaimTypes.WindowsDeviceGroup)?.Value,
            WindowsFqbnVersion = User.FindFirst(ClaimTypes.WindowsFqbnVersion)?.Value,
            WindowsSubAuthority = User.FindFirst(ClaimTypes.WindowsSubAuthority)?.Value
        });
        
        /*
        Ok(new {
            id    = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            email = User.FindFirst(ClaimTypes.Email)?.Value,
            role  = User.FindFirst(ClaimTypes.Role)?.Value,
        });*/
    
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var login = await authService.Login(loginRequestDto);

            if (login == null)
                return Unauthorized(new { menssage = "Invalid email or password!" });

        return Ok(login);
    }
}