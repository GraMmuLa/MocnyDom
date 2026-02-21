using Microsoft.EntityFrameworkCore;
using MocnyDom.Application.DTOs;
using MocnyDom.Application.Services;
using MocnyDom.Domain.Entities;
using MocnyDom.Infrastructure.Persistence;

namespace MocnyDom.Infrastructure.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly ApplicationDbContext _context;

        public BuildingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BuildingDto>> GetAllAsync()
        {
            var buildings = await _context.Buildings
                .Include(b => b.Floors)
                    .ThenInclude(f => f.Rooms)
                .ToListAsync();

            return buildings.Select(b => new BuildingDto
            {
                Id = b.Id,
                Name = b.Name,
                Floors = b.Floors
                    .Select(f => new FloorDto
                    {
                        Id = f.Id,
                        Number = f.Number,
                        Rooms = f.Rooms
                            .Select(r => new RoomDto
                            {
                                Id = r.Id,
                                Number = r.Number
                            }).ToList()
                    }).ToList()
            }).ToList();
        }

        public async Task AssignManagerAsync(string userId, int buildingId)
        {
            var building = await _context.Buildings.FindAsync(buildingId);
            if (building == null)
                throw new Exception("Building not found");

            var assigned = await _context.BuildingManagers
                .AnyAsync(x => x.UserId == userId && x.BuildingId == buildingId);

            if (assigned)
                throw new Exception("Manager already assigned");

            _context.BuildingManagers.Add(new BuildingManager
            {
                UserId = userId,
                BuildingId = buildingId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<BuildingDto>> GetForManagerAsync(string userId)
        {
            var query = _context.Buildings
                .Where(b => b.Managers.Any(m => m.UserId == userId))
                .Include(b => b.Floors)
                    .ThenInclude(f => f.Rooms);

            var buildings = await query.ToListAsync();

            return buildings.Select(b => new BuildingDto
            {
                Id = b.Id,
                Name = b.Name,
                Floors = b.Floors.Select(f => new FloorDto
                {
                    Id = f.Id,
                    Number = f.Number,
                    Rooms = f.Rooms.Select(r => new RoomDto
                    {
                        Id = r.Id,
                        Number = r.Number
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        public async Task<BuildingDto> CreateAsync(string name)
        {
            var building = new Building
            {
                Name = name
            };

            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Floors = new List<FloorDto>()
            };
        }

        // Implemented to satisfy IBuildingService
        public async Task<BuildingDto?> GetByIdAsync(int id)
        {
            var building = await _context.Buildings
                .Include(b => b.Floors)
                    .ThenInclude(f => f.Rooms)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null)
                return null;

            return new BuildingDto
            {
                Id = building.Id,
                Name = building.Name,
                Floors = building.Floors.Select(f => new FloorDto
                {
                    Id = f.Id,
                    Number = f.Number,
                    Rooms = f.Rooms.Select(r => new RoomDto
                    {
                        Id = r.Id,
                        Number = r.Number
                    }).ToList()
                }).ToList()
            };
        }
    }
}
