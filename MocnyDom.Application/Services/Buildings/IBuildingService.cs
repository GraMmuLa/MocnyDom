using MocnyDom.Application.DTOs;

namespace MocnyDom.Application.Services.Buildings
{
    public interface IBuildingService
    {
        Task<List<BuildingDto>> GetAllAsync();
        Task<BuildingDto> CreateAsync(string name);
        Task AssignManagerAsync(string userId, int buildingId);
        Task<List<BuildingDto>> GetForManagerAsync(string userId);
        Task<BuildingDto?> GetByIdAsync(int id);


    }
}
