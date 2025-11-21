namespace Hello_World.Models;

public class BirdCallRequest
{
    /// <summary>
    /// Name of the bird whose call is requested.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Whether to include a short description in the response.
    /// </summary>
    public bool IncludeDescription { get; set; } = true;
}