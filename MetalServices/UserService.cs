using AutoMapper;
using MetalModels;
using MetalModels.Models;
using MetalServices.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace MetalServices
{
    public class UserService : IUserService
    {
        private readonly MetalDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(MetalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserResponse> CreateNewUser(UserRequest request)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

            if (existingUser == null)
            {
                existingUser = new User
                {
                    Id = request.Id,
                    AndroidVersion = request.AndroidVersion,
                    MacAddress = request.MacAddress,
                    Smartphone = request.Smartphone
                };

                await _dbContext.Users.AddAsync(existingUser);
            }

            var userResponse = _mapper.Map<UserResponse>(existingUser);

            return userResponse;
        }
    }
}
