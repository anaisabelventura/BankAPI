using Azure.Core;
using BankAPI.Business;
using BankAPI.Business.Interfaces;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Authentication.Interface;
using BankAPI.Models.Requests;
using BankAPI.Models.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        private readonly postgresContext _context;
        private readonly IJwt _jwtAuth;
        public AccountController(IAccountBusiness accountBusiness, postgresContext context, IJwt jwtAuth)
        {
            _accountBusiness = accountBusiness;
            _context = context;
            _jwtAuth = jwtAuth;
        }

        // GET: <AccountController>
        [HttpGet(Name = "GetAccounts")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                //int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                int userId = int.Parse(_jwtAuth.GetClaim(authToken: Request.Headers.Authorization, claimName: "user"));

                var accounts = await _accountBusiness.GetAccountsByUserId(userId);
                var accountResponseList = AccountResponse.FromListAccountsUser(accounts);
                return StatusCode(StatusCodes.Status201Created, accountResponseList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        // GET <AccountController>/5
        [HttpGet("{id:int}", Name = "GetAccount")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[ProducesResponseType(typeof(IEnumerable<AccountMovims>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAccountById([FromRoute] int id)
        {
            try
            {
                int userId = int.Parse(_jwtAuth.GetClaim(authToken: Request.Headers.Authorization, claimName: "user"));
                var account = await _accountBusiness.GetById(id);
                //Get Account ID
                if (account.UserId != userId) return StatusCode(StatusCodes.Status401Unauthorized, "User don't have Owner from account");


                var accountResponse = AccountResponse.ToAcountResponse(account);
                return StatusCode(StatusCodes.Status200OK, accountResponse);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    ArgumentException => BadRequest(ex.Message),
                    AuthenticationException => Unauthorized(ex.Message),
                    _ => StatusCode(StatusCodes.Status400BadRequest, ex.Message)
                };
            }
        }

        // POST <AccountController>
        [HttpPost(Name = "CreateAccount")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCreatedAccount([FromBody] AccountRequest accountRequest)
        {
            try
            {
                int userId = int.Parse(_jwtAuth.GetClaim(authToken: Request.Headers.Authorization, claimName: "user"));

                var createdUser = await _accountBusiness.Create(accountRequest, userId);

                return StatusCode(StatusCodes.Status200OK, accountRequest);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    ArgumentException => BadRequest(ex.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, ex.Message)
                };
            }

        }
    }
}