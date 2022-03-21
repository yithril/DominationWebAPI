using AutoWrapper.Wrappers;

namespace Domination_WebAPI.Domain.Interface
{
    public interface IMapDomain
    {
        Task<ApiResponse> CreateGameZone(int x, int y);
        Task<ApiResponse> GetGameZoneById(int id);
    }
}
