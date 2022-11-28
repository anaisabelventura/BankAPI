using BankAPI.Business.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Application.Business;
using BankAPI.Models.Requests;
using BankAPI.Models.Responses;
using System.Runtime.CompilerServices;
using BankAPI.Business;
using static BankAPI.Models.Responses.RegistrationResponse;
using System.Security.Authentication;
using Azure.Core;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Application.Business.Interfaces;

namespace BankAPI.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private postgresContext _context;
        protected IUserBusiness _userBusiness;
        private readonly IJwt _jwt;
        protected ITokenBusiness _ItokenManager;


        public UserController(IUserBusiness userBusiness, postgresContext context, IJwt jwt, ITokenBusiness tokenManager)
        {
            _userBusiness = userBusiness;

            _context = context;

            _jwt = jwt;

            _ItokenManager = tokenManager;

        }

        [HttpPost(Name = "CreateUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RegistrationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationRequest userRequest)
        {
            try
            {
                var createdUser = await _userBusiness.Create(userRequest);
                return StatusCode(StatusCodes.Status201Created, userRequest);
            }
            catch (InvalidCastException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Bad Request");
            }

        }
    }
}
