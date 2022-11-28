using BankAPI.Business.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BankAPI.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Business;
using BankAPI.Models;
using BankAPI.Models.Requests;
using BankAPI.Models.Responses;
using System.Security.Authentication;
using BankAPI.Core.Models;
using AutoMapper;
using static StackExchange.Redis.Role;
using BankAPI.Infrastructure.Authentication.Interface;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly postgresContext _context;
        private readonly ITransferBusiness _transferBusiness;
        private readonly IJwt _jwtAuth;

        public TransferController(ITransferBusiness transferBusiness, postgresContext context, IJwt jwt)
        {
            _transferBusiness = transferBusiness;
            _context = context;
            _jwtAuth = jwt;
        }

        // POST api/<TransferController>
        [HttpPost]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(TransferRequest transferRequest)
        {
            try
            {
                int userId = int.Parse(_jwtAuth.GetClaim(authToken: Request.Headers.Authorization, claimName: "user"));
                var response = await _transferBusiness.CreateTransfer(transferRequest, userId);
                if (response is null)
                {
                    return BadRequest(StatusCodes.Status400BadRequest);
                }
                return StatusCode(StatusCodes.Status200OK, response);
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
    }
}