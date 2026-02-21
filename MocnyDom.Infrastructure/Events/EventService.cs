using Microsoft.EntityFrameworkCore;
using MocnyDom.Application.DTOs;
using MocnyDom.Application.Email;
using MocnyDom.Application.Services;
using MocnyDom.Domain.Entities;
using MocnyDom.Domain.Enums;
using MocnyDom.Infrastructure.Persistence;

namespace MocnyDom.Infrastructure.Services.Events
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public EventService(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<EventDto> CreateExternalAsync(ExternalEventRequest request)
        {
            var sensor = await _context.Sensors
                .Include(s => s.Room)
                .ThenInclude(r => r.Floor)
                .ThenInclude(f => f.Building)
                .FirstOrDefaultAsync(s => s.Name == request.SensorId);

            if (sensor == null)
                throw new Exception("Sensor not found");

            var ev = new Event
            {
                SensorId = sensor.Id,
                Type = request.EventType,
                Value = request.Location,
                Status = EventStatus.NEW
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            var buildingId = sensor.Room.Floor.BuildingId;

            var managers = await _context.BuildingManagers
                .Include(bm => bm.User)
                .Where(bm => bm.BuildingId == buildingId)
                .Select(bm => bm.User.Email)
                .ToListAsync();

            if (managers.Count > 0)
            {
                var subject = $"ALERT: {ev.Type}";
                var body = request.Location ?? "Event detected";
                await _emailService.SendManyAsync(managers, subject, body);
            }

            return await MapToDto(ev.Id);
        }

        // Implemented missing interface member
        public async Task<EventDto> CreateAsync(string userId, int sensorId, EventType type, string? value)
        {
            var sensor = await _context.Sensors
                .Include(s => s.Room)
                .ThenInclude(r => r.Floor)
                .ThenInclude(f => f.Building)
                .FirstOrDefaultAsync(s => s.Id == sensorId);

            if (sensor == null)
                throw new Exception("Sensor not found");

            var ev = new Event
            {
                SensorId = sensor.Id,
                Type = type,
                Value = value,
                Status = EventStatus.NEW
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            var buildingId = sensor.Room.Floor.BuildingId;

            var managers = await _context.BuildingManagers
                .Include(bm => bm.User)
                .Where(bm => bm.BuildingId == buildingId)
                .Select(bm => bm.User.Email)
                .ToListAsync();

            if (managers.Count > 0)
            {
                var subject = $"ALERT: {ev.Type}";
                var body = value ?? "Event detected";
                await _emailService.SendManyAsync(managers, subject, body);
            }

            return await MapToDto(ev.Id);
        }

        public async Task<IEnumerable<EventDto>> GetForManagerAsync(int buildingId, string userId)
        {
            var assigned = await _context.BuildingManagers
                .AnyAsync(bm => bm.BuildingId == buildingId && bm.UserId == userId);

            if (!assigned)
                throw new Exception("No access");

            var evs = await _context.Events
                .Include(e => e.Sensor)
                .ThenInclude(s => s.Room)
                .ThenInclude(r => r.Floor)
                .ThenInclude(f => f.Building)
                .Where(e => e.Sensor.Room.Floor.BuildingId == buildingId)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();

            var list = new List<EventDto>();

            foreach (var e in evs)
                list.Add(await MapToDto(e.Id));

            return list;
        }

        // New: implement IEventService.GetDetailsAsync
        public async Task<EventDto> GetDetailsAsync(int eventId, string userId)
        {
            var ev = await _context.Events
                .Include(e => e.Sensor)
                .ThenInclude(s => s.Room)
                .ThenInclude(r => r.Floor)
                .ThenInclude(f => f.Building)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                throw new Exception("Event not found");

            var buildingId = ev.Sensor.Room.Floor.BuildingId;

            var assigned = await _context.BuildingManagers
                .AnyAsync(bm => bm.BuildingId == buildingId && bm.UserId == userId);

            if (!assigned)
                throw new Exception("No access");

            return await MapToDto(eventId);
        }

        private async Task<EventDto> MapToDto(int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.Sensor)
                .ThenInclude(s => s.Room)
                .ThenInclude(r => r.Floor)
                .ThenInclude(f => f.Building)
                .FirstAsync(e => e.Id == eventId);

            return new EventDto
            {
                Id = ev.Id,
                SensorId = ev.Sensor.Name,
                BuildingId = ev.Sensor.Room.Floor.BuildingId,
                ReportedAt = ev.Timestamp,
                EventType = ev.Type,
                Status = ev.Status,
                EmailSent = true,

                Location = $"{ev.Sensor.Room.Floor.Building.Name}/{ev.Sensor.Room.Number}",
                RoomNumber = ev.Sensor.Room.Number,
                FloorNumber = ev.Sensor.Room.Floor.Number,
                BuildingName = ev.Sensor.Room.Floor.Building.Name
            };
        }
    }
}
