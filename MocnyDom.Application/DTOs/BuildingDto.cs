using MocnyDom.Application.DTOs;

public class BuildingDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<FloorDto> Floors { get; set; } = new();
}
