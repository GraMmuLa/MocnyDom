using MocnyDom.Application.DTOs;
using MocnyDom.Domain.Enums;

namespace MocnyDom.Application.Services
{
    public interface IEventService
    {
        // Manager internal create (już istnieje)
        Task<EventDto> CreateAsync(string userId, int sensorId, EventType type, string? value);

        // External sensor JSON create
        Task<EventDto> CreateExternalAsync(ExternalEventRequest request);

        // Manager view - list events for building
        Task<IEnumerable<EventDto>> GetForManagerAsync(int buildingId, string userId);

        // Manager view - event details
        Task<EventDto> GetDetailsAsync(int eventId, string userId);
    }
}
