using MocnyDom.Application.DTOs;

namespace MocnyDom.Application.Services
{
    public interface ISensorService
    {
        Task<SensorDto> CreateAsync(string userId, int roomId, string name, string type);

        Task<IEnumerable<SensorDto>> GetForManagerAsync(int buildingId, string userId);
    }
}
