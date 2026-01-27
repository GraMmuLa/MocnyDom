using Microsoft.EntityFrameworkCore;
using MocnyDom.Application.DTOs;
using MocnyDom.Domain.Entities;
using MocnyDom.Infrastructure.Persistence;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;

    public RoomService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoomDto> CreateAsync(string userId, int floorId, string name)
    {
        var floor = await _context.Floors
            .Include(f => f.Building)
            .FirstOrDefaultAsync(f => f.Id == floorId);

        if (floor == null)
            throw new Exception("Floor not found");

        var assigned = await _context.BuildingManagers
            .AnyAsync(bm => bm.UserId == userId && bm.BuildingId == floor.BuildingId);

        if (!assigned)
            throw new Exception("Manager not assigned to this building");

        var room = new Room
        {
            FloorId = floorId,
            Number = name // tu byłby number ale pod B jest string
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return new RoomDto
        {
            Id = room.Id,
            Number = room.Number
        };
    }
}
