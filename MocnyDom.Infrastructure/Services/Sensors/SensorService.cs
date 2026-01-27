using Microsoft.EntityFrameworkCore;
using MocnyDom.Application.DTOs;
using MocnyDom.Application.Services.Sensors;
using MocnyDom.Domain.Enums;
using MocnyDom.Infrastructure.Persistence;

public class SensorService : ISensorService
{
    private readonly ApplicationDbContext _context;

    public SensorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SensorDto> CreateAsync(string userId, int roomId, string name, string type)
    {
        var room = await _context.Rooms
            .Include(r => r.Floor)
            .ThenInclude(f => f.Building)
            .FirstAsync(r => r.Id == roomId);

        if (!Enum.TryParse<SensorType>(type, true, out var sensorType))
            throw new Exception("Invalid type");

        var sensor = new Sensor
        {
            RoomId = roomId,
            Name = name,
            Type = sensorType
        };

        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        return await Map(sensor.Id);
    }

    public async Task<IEnumerable<SensorDto>> GetForManagerAsync(int buildingId, string userId)
    {
        var sensors = await _context.Sensors
            .Include(s => s.Room)
            .ThenInclude(r => r.Floor)
            .ThenInclude(f => f.Building)
            .Where(s => s.Room.Floor.BuildingId == buildingId)
            .ToListAsync();

        return await Task.WhenAll(sensors.Select(x => Map(x.Id)));
    }

    private async Task<SensorDto> Map(int id)
    {
        var s = await _context.Sensors
            .Include(s => s.Room)
            .ThenInclude(r => r.Floor)
            .ThenInclude(f => f.Building)
            .FirstAsync(s => s.Id == id);

        return new SensorDto
        {
            Id = s.Id,
            ExternalSensorId = s.Name,
            RoomId = s.RoomId,
            Type = s.Type,
            LocationDescription = $"{s.Room.Floor.Building.Name} - Floor {s.Room.Floor.Number} - Room {s.Room.Number}",
            FloorNumber = s.Room.Floor.Number,
            BuildingName = s.Room.Floor.Building.Name
        };
    }
}
