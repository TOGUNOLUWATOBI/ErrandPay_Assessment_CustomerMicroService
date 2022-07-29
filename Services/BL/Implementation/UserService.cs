using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs;
using Models.Entities;
using Models.Models;
using Repository;
using Services.BL.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.BL.Implementation
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        
        public UserService(IRepository<User> repository, IMapper mapper,UserManager<User> userManager,IConfiguration configuration,RoleManager<IdentityRole> roleManager)
        {
            _userRepo = repository;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<BaseResponseDTO> CreateUser(UserDTO model)
        {
            var baseResponseDTO = new BaseResponseDTO();
            if (model != null)
            {
                var userexist = await _userManager.FindByEmailAsync(model.Email);
                if(userexist == null)
                {
                    User user = new User()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                       
                        
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if(result.Succeeded)
                    {
                        if(! await _roleManager.RoleExistsAsync(UserRoles.User))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                        }
                        if(await _roleManager.RoleExistsAsync(UserRoles.User))
                        {
                            await _userManager.AddToRoleAsync(user,UserRoles.User);
                        }

                        baseResponseDTO.IsSuccessful = result.Succeeded;
                        baseResponseDTO.Message = result.Succeeded ? "User created" : "Couldn't create user";
                    }
                }                
            }
            return baseResponseDTO;
        }

        public async Task<BaseResponseDTO> CreateAdmin(UserDTO model)
        {
            var baseResponseDTO = new BaseResponseDTO();
            if (model != null)
            {
                var userexist = await _userManager.FindByEmailAsync(model.Email);
                if (userexist == null)
                {
                    User user = new User()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.UserName,


                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
                        }
                        if (await _roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
                        {
                            await _userManager.AddToRoleAsync(user, UserRoles.SuperAdmin);
                        }

                        baseResponseDTO.IsSuccessful = result.Succeeded;
                        baseResponseDTO.Message = result.Succeeded ? "SuperAdmin created" : "Couldn't create SuperAdmin";
                    }
                }
            }
            return baseResponseDTO;
        }

        public async Task<BaseResponseDTO> CreateStaff(UserDTO model)
        {
            var baseResponseDTO = new BaseResponseDTO();
            if (model != null)
            {
                var userexist = await _userManager.FindByEmailAsync(model.Email);
                if (userexist == null)
                {
                    User user = new User()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.UserName,


                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(UserRoles.Staff))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));
                        }
                        if (await _roleManager.RoleExistsAsync(UserRoles.Staff))
                        {
                            await _userManager.AddToRoleAsync(user, UserRoles.Staff);
                        }

                        baseResponseDTO.IsSuccessful = result.Succeeded;
                        baseResponseDTO.Message = result.Succeeded ? "Staff created" : "Couldn't create staff";
                    }
                }
            }
            return baseResponseDTO;
        }


        public async Task<OperationResponseDTO<User>> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null && await _userManager.CheckPasswordAsync(user,model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer:_configuration["JWT:ValidIssuer"],
                    audience:_configuration["JWT:ValidAudience"],
                    expires:DateTime.Now.AddHours(3),
                    claims:authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return new OperationResponseDTO<User>
                {
                    IsSuccessful = true,
                    data = user,
                    Message = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            return new OperationResponseDTO<User>
            {
                IsSuccessful = false,
                data = null,
                Message = "failed"
            };
        }

        public Task<OperationResponseDTO<User>> DeleteUser(string email)
        {
            var operationResponseDto = new OperationResponseDTO<User>();
            var user = _userRepo.FindByEmail(email);
            var status = _userRepo.Delete(user);
            operationResponseDto.IsSuccessful = status;
            operationResponseDto.Message = status ? "User Deleted" : "COuldn't Delete User";
            operationResponseDto.data = user;
            return Task.FromResult(operationResponseDto);

        }

        public Task<OperationResponseDTO<User>> GetUser(string email)
        {
            var user = _userRepo.FindByEmail(email);
            var operationResponseDto = new OperationResponseDTO<User>()
            {
                data = user,
                IsSuccessful = user != null,
                Message = user != null ? "User Found" : "User Not Found"
            };

            return Task.FromResult(operationResponseDto);
        }

        public Task<BaseResponseDTO> UpdateUser(string id,UserDTO model)
        {
            BaseResponseDTO baseResponseDTO = new BaseResponseDTO();
            if(model != null)
            {
                var userToBeUpdated = _userRepo.FindById(id);
                if(userToBeUpdated != null)
                {
                    userToBeUpdated.PasswordHash = !string.IsNullOrEmpty(model.Password) ? model.Password : userToBeUpdated.PasswordHash;
                    userToBeUpdated.Email = !string.IsNullOrEmpty(model.Email) ? model.Email : userToBeUpdated.Email ;
                    userToBeUpdated.UserName = !string.IsNullOrEmpty(model.UserName) ? model.UserName : userToBeUpdated.UserName;
                    

                    var updated = _userRepo.Update(userToBeUpdated);
                    baseResponseDTO.Message = "Successful";
                    baseResponseDTO.IsSuccessful = updated;


                }
            }
            return Task.FromResult(baseResponseDTO);

        }
    }
}
