using Models.DTOs;
using Models.Entities;
using Models.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.BL.Interface
{
    public interface IUserService : IAutoDependencyService
    {
        Task<BaseResponseDTO> CreateUser(UserDTO model);
        Task<BaseResponseDTO> UpdateUser(string id,UserDTO model);
        Task<OperationResponseDTO<User>> DeleteUser(string email);
        Task<OperationResponseDTO<User>> GetUser(string email);
        Task<OperationResponseDTO<User>> Login(LoginModel model);
        Task<BaseResponseDTO> CreateStaff(UserDTO model);
        Task<BaseResponseDTO> CreateAdmin(UserDTO model);
    }
}
