using MocnyDom.Application.DTOs;

public interface IRoomService
{
    Task<RoomDto> CreateAsync(string userId, int floorId, string name);
}
