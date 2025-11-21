using System;

namespace Hello_World.Models;

public class BirdSighting
{
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// A nickname or handle of the spotter.
    /// </summary>
    public required string Observer { get; init; }

    /// <summary>
    /// The bird that was seen.
    /// </summary>
    public required Bird Bird { get; init; }

    /// <summary>
    /// Rough location or description (e.g., "City Park Lake").
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// When the sighting occurred.
    /// </summary>
    public DateTimeOffset ObservedAt { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Optional free-form notes.
    /// </summary>
    public string? Notes { get; init; }
}