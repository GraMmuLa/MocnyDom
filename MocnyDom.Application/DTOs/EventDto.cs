using System;
using MocnyDom.Domain.Enums;

namespace MocnyDom.Application.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }

        // Front model
        public string SensorId { get; set; }
        public int BuildingId { get; set; }
        public DateTime ReportedAt { get; set; }
        public string Location { get; set; }
        public EventType EventType { get; set; }
        public EventStatus Status { get; set; }
        public bool EmailSent { get; set; }

        // Backend internal (nie przeszkadza)
        public string RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public string BuildingName { get; set; }
    }
}
