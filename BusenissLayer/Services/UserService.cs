using BusinessLayer.Interfaces.Repository;
using BusinessLayer.Interfaces.Services;
using Common.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataLayer;

namespace BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> CreateAsync(UserDTO model)
        {
            if (model == null ||
                string.Equals(model.Name, "string", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(model.LastName, "string", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(model.Email, "string", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var existing = await _userRepository.GetByEmailAsync(model.Email);
            if (existing != null) return false;

            var userEntity = new UserApp.DataLayer.Entities.UserEntity
            {
                PublicId = model.PublicId == Guid.Empty ? Guid.NewGuid() : model.PublicId,
                Name = model.Name,
                Email = model.Email,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Role = model.Role,
                PasswordHash = model.Password
            };

            await _userRepository.CreateAsync(userEntity);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid publicId)
        {
            var userEntity = await _userRepository.GetByPublicIdAsync(publicId);
            if (userEntity == null) return false;

            _userRepository.Delete(userEntity);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRangeAsync(List<Guid> userPublicIds)
        {
            if (userPublicIds == null || userPublicIds.Count == 0) return false;
            
            var userEntities = new List<UserApp.DataLayer.Entities.UserEntity>();
            foreach (var publicId in userPublicIds)
            {
                var userEntity = await _userRepository.GetByPublicIdAsync(publicId);
                if (userEntity != null)
                {
                    userEntities.Add(userEntity);
                }
            }
            
            if (userEntities.Count > 0)
            {
                _userRepository.DeleteRange(userEntities);
                await _userRepository.SaveChangesAsync();
            }
            
            return true;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var userList = await _userRepository.GetAllAsync();
            var userDTOList = new List<UserDTO>();

            foreach (var user in userList)
            {
                userDTOList.Add(new UserDTO
                {
                    PublicId = user.PublicId,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Role = user.Role
                });
            }

            return userDTOList;
        }

        public async Task<UserDTO> GetByPublicIdAsync(Guid userPublicId)
        {
            var userEntity = await _userRepository.GetByPublicIdAsync(userPublicId);

            if (userEntity == null) return null;

            return new UserDTO
            {
                PublicId = userEntity.PublicId,
                Name = userEntity.Name,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                DateOfBirth = userEntity.DateOfBirth,
                PhoneNumber = userEntity.PhoneNumber,
                Address = userEntity.Address,
                Role = userEntity.Role
            };
        }

        public async Task<bool> UpdateAsync(UserDTO model)
        {
            var userEntity = await _userRepository.GetByPublicIdAsync(model.PublicId);
            if (userEntity == null) return false;
            
            if (string.Equals(model.Name, "string", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(model.LastName, "string", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(model.Email, "string", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            userEntity.Name = model.Name;
            userEntity.Email = model.Email;
            userEntity.LastName = model.LastName;
            userEntity.DateOfBirth = model.DateOfBirth;
            userEntity.PhoneNumber = model.PhoneNumber;
            userEntity.Address = model.Address;
            userEntity.Role = model.Role;
            
            _userRepository.Update(userEntity);
            await _userRepository.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ResetPasswordAsync(Guid userPublicId, string newPassword)
        {
            var userEntity = await _userRepository.GetByPublicIdAsync(userPublicId);
            if (userEntity == null) return false;

            userEntity.PasswordHash = newPassword;
            _userRepository.Update(userEntity);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<UserDTO> AuthenticateAsync(string email, string password)
        {
            var userList = await _userRepository.GetAllAsync();
            var userEntity = userList.FirstOrDefault(u => u.Email == email);

            if (userEntity == null)
                return null;

            if (userEntity.PasswordHash != password)
                return null;

            return new UserDTO
            {
                PublicId = userEntity.PublicId,
                Name = userEntity.Name,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                DateOfBirth = userEntity.DateOfBirth,
                PhoneNumber = userEntity.PhoneNumber,
                Address = userEntity.Address,
                Role = userEntity.Role
            };
        }
    }
}
