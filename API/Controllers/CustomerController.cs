using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Models;
using Serilog;
using Services.BL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    
    
    
    public class CustomerController : ControllerBase
    {
        private readonly IUserService _userService;
        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = UserRoles.User+","+UserRoles.Staff+","+UserRoles.SuperAdmin)]
        [HttpPost("Onboarding/User/CreateUser")]
        
        public async Task<IActionResult> CreateUser(UserDTO model)
        {
            var response = await _userService.CreateUser(model);
            Log.Warning("UserIntegrationResponse : {@Response}", response);
            var finalResp = await GetNameAndRole(HttpContext.User.Identity as ClaimsIdentity);

            return Ok(finalResp);
        }

        //made it anonyous so as to be able to create an adminUser and run the app
        [AllowAnonymous]
        [HttpPost("Onboarding/User/CreateAdmin")]
        
        public async Task<IActionResult> CreateAdmin(UserDTO model)
        {
            var response = await _userService.CreateAdmin(model);
            Log.Warning("UserIntegrationResponse : {@Response}", response);

            var finalResp = await GetNameAndRole(HttpContext.User.Identity as ClaimsIdentity);

            return Ok(finalResp);
        }

        [Authorize(Roles = UserRoles.Staff + "," + UserRoles.SuperAdmin)]
        [HttpPost("Onboarding/User/CreateStaff")]
        public async Task<IActionResult> CreateStaff(UserDTO model)
        {
            var response = await _userService.CreateStaff(model);
            Log.Warning("UserIntegrationResponse : {@Response}", response);

            var finalResp = await GetNameAndRole(HttpContext.User.Identity as ClaimsIdentity);

            return Ok(finalResp);
        }

        [Authorize(Roles = UserRoles.SuperAdmin)]
        [HttpGet("Onboarding/User/Find/{id}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var resp = await _userService.GetUser(email);
            Log.Warning("UserIntegrationResponse : {@Response}", resp);
            return Ok(resp);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var respone = await _userService.Login(model);
            return Ok(
                new
                {
                    token = respone.Message,
                    user = respone.data
                });
        }

        private async Task<FinalResponseDTO> GetNameAndRole(ClaimsIdentity identity)
        {
            var finaleResponse  = new FinalResponseDTO();

            // Gets list of claims.
            IEnumerable<Claim> claim = identity.Claims;

            // Gets name from claims. Generally it's an email address.
            finaleResponse.Name = claim
                .Where(x => x.Type == ClaimTypes.Name)
                .FirstOrDefault().Value;
            finaleResponse.Role = claim
                .Where(x => x.Type == ClaimTypes.Role)
                .FirstOrDefault().Value;
            return finaleResponse;
        }
    }
}
