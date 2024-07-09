using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using opensystem_api.Helpers;
using opensystem_api.Models;
using opensystem_api.Services;
using OpenSystem_Api.Helpers;

namespace opensystem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAuthService authService, ILogger<AdminController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ConstantsEnums.Url.Users)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

       /* [HttpPost(ConstantsEnums.Url.CreateUser)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new ResponseResult<string>((int)StatusCodesEnum.BadRequest, false, ConstantsEnums.Strings.InvalidInput,null));

            var result = await _authService.CreateUserAsync(request);

            if (!result.Success)
                return StatusCode((int)StatusCodesEnum.InternalServerError, new ResponseResult<string>((int)StatusCodesEnum.InternalServerError, false, ConstantsEnums.Strings.ErrorMessage, result.Message));

            return Ok(result);
        }

        [HttpDelete(ConstantsEnums.Url.DeleteUser)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _authService.DeleteUserAsync(id);

            if (!result.Success)
                return StatusCode((int)StatusCodesEnum.InternalServerError, new ResponseResult<string>((int)StatusCodesEnum.InternalServerError, false, ConstantsEnums.Strings.ErrorMessage, result.Message));

            return Ok(result);
        }*/
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IWebHostEnvironment env, IAuthService authService, ILogger<AuthController> logger)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(ConstantsEnums.Url.Environment)]
        public ActionResult<string> GetEnvironment()
        {
            return Ok($"The application is running in {_env.EnvironmentName} environment.");
        }

        [HttpPost(ConstantsEnums.Url.ForgotPassword)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
                return BadRequest(new { message = "Email is required" });

            var result = await _authService.ForgotPassword(request.Email);

            if (!result.Success)
                return StatusCode((int)StatusCodesEnum.InternalServerError, new { message = result.Message });

            return Ok(new { message = result.Data }); 
        }

        [HttpPost(ConstantsEnums.Url.Login)]
        public async Task<ActionResult<ResponseResult<string>>> Login(LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
                return BadRequest(new ResponseResult<string>((int)StatusCodesEnum.BadRequest, false, string.Empty, ConstantsEnums.Strings.InvalidInput));

            _logger.LogInformation($"Login attempt for user: {loginModel.Email}");

            try
            {
                // Attempt to authenticate user
                var authResult = await _authService.AuthenticateAsync(loginModel.Email, loginModel.Password);

                if (authResult.Success)
                    return Ok(authResult);
                else
                {
                    _logger.LogWarning($"Unauthorized login attempt for user: {loginModel.Email}");
                    return Unauthorized(authResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login for user: {loginModel.Email}. Error: {ex.Message}");
                _logger.LogError($"Exception details: {ex.StackTrace}");
                return StatusCode((int)StatusCodesEnum.InternalServerError, new ResponseResult<string>((int)StatusCodesEnum.InternalServerError, false, string.Empty, ConstantsEnums.Strings.ErrorMessage));
            }
        }





    }
}
