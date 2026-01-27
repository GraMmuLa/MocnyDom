using MocnyDom.Domain.Enums;

namespace MocnyDom.Application.DTOs
{
    public class SensorDto
    {
        public int Id { get; set; }

        // Front expects this
        public string ExternalSensorId { get; set; }

        public int RoomId { get; set; }
        public SensorType Type { get; set; }

        public string LocationDescription { get; set; }

        // Useful internal
        public int FloorNumber { get; set; }
        public string BuildingName { get; set; }
    }
}
