using Microsoft.AspNetCore.Identity;
using MocnyDom.Domain.Entities;

public class BuildingManager
{
    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public int BuildingId { get; set; }
    public Building Building { get; set; }
}
