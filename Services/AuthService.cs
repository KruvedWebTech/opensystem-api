
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using opensystem_api.Data;
using opensystem_api.Middleware;
using Newtonsoft.Json;
using opensystem_api.Models;

using AutoMapper;

using OpenSystem_Api.Helpers;
using opensystem_api.Helpers;

namespace opensystem_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(ILogger<AuthService> logger, IConfiguration configuration, AppDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _context = dbContext;
            _mapper = mapper;
        }


        public async Task<ResponseResult<string>> ForgotPassword(string email)
        {
            _logger.LogInformation($"Password reset requested for email: {email}");

            try
            {
                var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Smtp:Port"]),
                    Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:FromEmail"]),
                    Subject = "Password Reset Request",
                    Body = "<h1>Password Reset</h1><p>Click <a href='http://example.com/reset-password'>here</a> to reset your password</p>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);

                return new ResponseResult<string>((int)StatusCodesEnum.Success, true, ConstantsEnums.Strings.SuccessMessage, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending password reset email: {ex.Message}");
                return new ResponseResult<string>((int)StatusCodesEnum.Forbidden, false, ConstantsEnums.Strings.UnauthorizedAccess, null);
            }
        }

        public async Task<ResponseResult<string>> AuthenticateAsync(string email, string password)
        {
            try
            {
                _logger.LogInformation($"Attempting authentication for email: {email}");

                // Query the database to check if the user exists
                var user = await _context.User
                 .Include(u => u.Role)
                 .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && !string.IsNullOrEmpty(u.Password) && u.Password == password);

                if (user != null)
                {
                    _logger.LogInformation($"User authenticated: {user.Email}");
                    var roleName = user.Role?.RoleName ?? "User"; // Assuming a default role if RoleName is null
                    var token = GenerateJwtToken(user); // Generate JWT token


                    return new ResponseResult<string>((int)StatusCodesEnum.Success, true, ConstantsEnums.Strings.SuccessMessage, token);
                }

                _logger.LogInformation($"Invalid credentials for user: {user?.Email ?? email}");
                return new ResponseResult<string>((int)StatusCodesEnum.Unauthorized, false, ConstantsEnums.Strings.InvalidInput, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during authentication for email: {email}. Error: {ex.Message}");
                return new ResponseResult<string>((int)StatusCodesEnum.InternalServerError, false, ConstantsEnums.Strings.ErrorMessage, null);
            }
        }




        public string GenerateJwtToken(User user)
        {
            // Map User object to a dictionary of claims using AutoMapper
            var claimsDictionary = _mapper.Map<Dictionary<string, string>>(user);

            // Serialize user object to JSON
            var userData = JsonConvert.SerializeObject(claimsDictionary);

            // Create claims for JWT token
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.Role?.RoleName ?? "User"),
            new Claim("userData", userData) // Serialized user data as a single claim
        };

            // Create a symmetric security key and credentials for signing the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token with issuer, audience, claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static string GenerateToken(HttpContext context, Dictionary<string, string> claimData, int expireMinutes = 30)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (string key2 in claimData.Keys)
            {
                dictionary.Add(key2, claimData[key2]);
            }

            var encryptionKey = "oWj8ioPt2%Xg93o57zXo";
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(encryptionKey));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            DateTime value = DateTime.Now.AddYears(1);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = dictionary,
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("jti", Guid.NewGuid().ToString("n"))
                }),
                Audience = AuthenticationMiddleware.GetClientIPAddress(context),
                Expires = value,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = signingCredentials
            };
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.User.Include(u => u.Role).Include(u => u.Company).ToListAsync();
        }

        public async Task<ResponseResult<string>> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = request.RoleId,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return new ResponseResult<string>((int)StatusCodesEnum.Success, true, ConstantsEnums.Strings.SuccessMessage, user.UserId.ToString());
        }

        public async Task<ResponseResult<string>> DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return new ResponseResult<string>((int)StatusCodesEnum.NotFound, false, ConstantsEnums.Strings.NOTFOUND, null);
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return new ResponseResult<string>((int)StatusCodesEnum.Success, true, ConstantsEnums.Strings.SuccessMessage, null);
        }
    }


}
