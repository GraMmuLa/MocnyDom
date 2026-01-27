using Microsoft.Extensions.Logging;
using MocnyDom.Domain.Entities;
using MocnyDom.Domain.Enums;

public class Sensor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SensorType Type { get; set; }

    public int RoomId { get; set; }
    public Room Room { get; set; }

    public List<Event> Events { get; set; } = new();

}
