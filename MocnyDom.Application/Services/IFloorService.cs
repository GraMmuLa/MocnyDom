using MocnyDom.Application.DTOs;

public interface IFloorService
{
    Task<FloorDto> CreateAsync(string userId, int buildingId, int number);
    Task<IEnumerable<FloorDto>> GetForManagerAsync(int buildingId, string userId);
    Task<FloorDto> CreateAsync(int buildingId, CreateFloorRequest request, string userId);

}
