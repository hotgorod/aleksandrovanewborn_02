using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AleksandrovaNewborn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AleksandrovaNewborn.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtBearerTokenSettings _jwtBearerTokenSettings;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager)
    {
        _jwtBearerTokenSettings = jwtTokenOptions.Value;
        _userManager = userManager;
    }

    /// <summary>
    /// Logs in new client and return authentication token. 
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("token")]
    public async Task<IActionResult> Login([FromBody] LoginCredentials credentials)
    {
        IdentityUser identityUser;

        if (!ModelState.IsValid
            || credentials == null
            || (identityUser = await ValidateUser(credentials)) == null)
        {
            return new BadRequestObjectResult(new { Message = "Login failed" });
        }

        var roles = await _userManager.GetRolesAsync(identityUser);

        var token = GenerateToken(identityUser, roles);
        return Ok(new { Token = token, Message = "Success" });
    }

    private async Task<IdentityUser> ValidateUser(LoginCredentials credentials)
    {
        var identityUser = await _userManager.FindByEmailAsync(credentials.Email);
        if (identityUser != null)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
            return result == PasswordVerificationResult.Failed ? null : identityUser;
        }

        return null;
    }

    private object GenerateToken(IdentityUser identityUser, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtBearerTokenSettings.SecretKey);

        var claims = new List<Claim>();
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.Add(new Claim(ClaimTypes.Name, identityUser.UserName));
        claims.Add(new Claim(ClaimTypes.Email, identityUser.Email));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(_jwtBearerTokenSettings.ExpiryTimeInSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtBearerTokenSettings.Audience,
            Issuer = _jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
