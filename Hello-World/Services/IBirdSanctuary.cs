using System.Collections.Generic;
using Hello_World.Models;

namespace Hello_World.Services;

/// <summary>
/// Represents an in-memory "sanctuary" of birds and recorded sightings.
/// In a real application this would likely be backed by a database.
/// </summary>
public interface IBirdSanctuary
{
    IReadOnlyCollection<Bird> GetAllBirds();
    Bird? GetBirdByName(string name);
    IReadOnlyCollection<BirdSighting> GetRecentSightings(int max = 25);
    BirdSighting AddSighting(string observer, string birdName, string location, string? notes);
}