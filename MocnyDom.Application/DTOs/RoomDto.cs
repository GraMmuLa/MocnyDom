using System.Collections.Generic;

namespace MocnyDom.Application.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public List<SensorDto> Sensors { get; set; } = new();
    }
}
