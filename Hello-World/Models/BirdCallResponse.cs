namespace Hello_World.Models;

public class BirdCallResponse
{
    public required string Name { get; init; }
    public required string Call { get; init; }
    public string? Description { get; init; }
}