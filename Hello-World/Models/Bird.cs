using System;
using System.Text.Json.Serialization;

namespace Hello_World.Models;

public class Bird
{
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Common name of the bird (e.g., "Peregrine Falcon").
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Latin / scientific name (e.g., "Falco peregrinus").
    /// </summary>
    public required string ScientificName { get; init; }

    /// <summary>
    /// Typical habitat or region (e.g., "Coastal cliffs", "Urban", "Wetlands").
    /// </summary>
    public required string Habitat { get; init; }

    /// <summary>
    /// Relative wingspan in centimeters.
    /// </summary>
    public int WingspanCm { get; init; }

    /// <summary>
    /// Primary diet (e.g., "Seeds", "Insects", "Fish").
    /// </summary>
    public required string Diet { get; init; }

    /// <summary>
    /// A short fact or description about the bird.
    /// </summary>
    public required string FunFact { get; init; }

    /// <summary>
    /// Overall rarity / conservation feeling (e.g., "Common", "Uncommon", "Rare").
    /// </summary>
    public required string Rarity { get; init; }

    /// <summary>
    /// A simple "call" representation.
    /// </summary>
    public required string Call { get; init; }

    [JsonIgnore]
    public string NameAndCall => $"{Name} – {Call}";
}