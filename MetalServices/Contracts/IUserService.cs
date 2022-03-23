using Shared.Dtos.Requests;
using Shared.Dtos.Responses;

namespace MetalServices.Contracts
{
    public interface IUserService
    {
        Task<UserResponse> CreateNewUser(UserRequest request);
    }
}
