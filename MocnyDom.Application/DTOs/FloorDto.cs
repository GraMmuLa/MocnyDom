using MocnyDom.Application.DTOs;

public class FloorDto
{
    public int Id { get; set; }
    public int Number { get; set; }
    public List<RoomDto> Rooms { get; set; } = new();
}
