using System;
using MocnyDom.Domain.Enums;

namespace MocnyDom.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }

        public EventType Type { get; set; }

        public string? Value { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public EventStatus Status { get; set; } = EventStatus.NEW;

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
