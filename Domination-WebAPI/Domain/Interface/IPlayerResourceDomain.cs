using AutoWrapper.Wrappers;
using Domination_WebAPI.Enum;

namespace Domination_WebAPI.Domain.Interface
{
    public interface IPlayerResourceDomain
    {
        Task<ApiResponse> UpdatePlayerResources(int UserId);
        Task<ApiResponse> SetNodeType(int nodeId, ResourceTypeEnum type, int userId);
        Task<ApiResponse> SetNodeImprovement(int id, int improvementId, int userId);
    }
}
