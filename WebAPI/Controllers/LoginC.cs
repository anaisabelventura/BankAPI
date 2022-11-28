using BankAPI.Application.Business.Interfaces;
using BankAPI.Business.Interfaces;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Models.Requests;
using BankAPI.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly postgresContext _context;
        private readonly IUserBusiness _userBusiness;
        private readonly IJwt _jwtAuth;
        private readonly ITokenBusiness _ITokenBusiness;

        public LoginController(postgresContext context, IUserBusiness userBusiness, IJwt jwtAuth, ITokenBusiness iTokenBusiness)
        {
            _context = context;
            _userBusiness = userBusiness;
            _jwtAuth = jwtAuth;
            _ITokenBusiness = iTokenBusiness;
        }

        [HttpPost("login", Name = "Login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest userRequest)
        {
            try
            {
                var response = await _userBusiness.Login(userRequest);

                var userResponse = new LoginResponse(response.Item1, response.Item2, response.Item3, response.Item4, response.Item5);
                return StatusCode(StatusCodes.Status200OK, userResponse);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    ArgumentException => BadRequest(ex.Message),
                    InvalidCastException => Unauthorized(ex.Message),
                    _ => StatusCode(StatusCodes.Status400BadRequest, ex.Message)
                };
            }
        }


        [HttpPost("revalidate", Name = "Revalidate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> RevalidateUser([FromBody] RenewRequest revalid)
        {

            int userId = int.Parse(_jwtAuth.GetClaim(authToken: Request.Headers.Authorization, claimName: "user"));

            var response = await _ITokenBusiness.Revalidate(userId, revalid);

            var userResponse = new LoginResponse(response.Item1, response.Item2, response.Item3, response.Item4, response.Item5);
            return StatusCode(StatusCodes.Status201Created, userResponse);

        }

    }
}
