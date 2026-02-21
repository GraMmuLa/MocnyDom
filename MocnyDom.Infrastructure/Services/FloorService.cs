using MocnyDom.Application.DTOs;
using MocnyDom.Domain.Entities;
using MocnyDom.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MocnyDom.Application.DTOs;
using Microsoft.EntityFrameworkCore;

public class FloorService : IFloorService
{
    private readonly ApplicationDbContext _context;

    public FloorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FloorDto> CreateAsync(string userId, int buildingId, int number)
    {
        var assigned = await _context.BuildingManagers
            .AnyAsync(bm => bm.UserId == userId && bm.BuildingId == buildingId);

        if (!assigned)
            throw new Exception("Manager not assigned to this building");

        var floor = new Floor
        {
            BuildingId = buildingId,
            Number = number
        };

        _context.Floors.Add(floor);
        await _context.SaveChangesAsync();

        return new FloorDto
        {
            Id = floor.Id,
            Number = floor.Number,
            Rooms = new List<RoomDto>()
        };
    }
    public async Task<IEnumerable<FloorDto>> GetForManagerAsync(int buildingId, string userId)
    {
        var hasAccess = await _context.BuildingManagers
            .AnyAsync(x => x.BuildingId == buildingId && x.UserId == userId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("Brak dostępu do budynku");

        return await _context.Floors
            .Where(f => f.BuildingId == buildingId)
            .Include(f => f.Rooms)
            .Select(f => new FloorDto
            {
                Id = f.Id,
                Number = f.Number,
                Rooms = f.Rooms.Select(r => new RoomDto
                {
                    Id = r.Id,
                    Number = r.Number
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<FloorDto> CreateAsync(int buildingId, CreateFloorRequest dto, string userId)
    {
        var hasAccess = await _context.BuildingManagers
            .AnyAsync(x => x.BuildingId == buildingId && x.UserId == userId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("Brak dostępu do budynku");

        var floor = new Floor
        {
            Number = dto.Number,
            BuildingId = buildingId
        };

        _context.Floors.Add(floor);
        await _context.SaveChangesAsync();

        return new FloorDto
        {
            Id = floor.Id,
            Number = floor.Number,
            Rooms = new List<RoomDto>()
        };
    }

}
