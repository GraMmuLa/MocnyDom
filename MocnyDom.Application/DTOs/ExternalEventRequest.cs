using MocnyDom.Domain.Enums;

namespace MocnyDom.Application.DTOs
{
    public class ExternalEventRequest
    {
        public string SensorId { get; set; }
        public string Location { get; set; }
        public EventType EventType { get; set; }
    }
}
