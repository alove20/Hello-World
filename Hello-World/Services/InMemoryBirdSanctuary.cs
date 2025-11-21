using System;
using System.Collections.Generic;
using System.Linq;
using Hello_World.Models;

namespace Hello_World.Services;

/// <summary>
/// Simple in-memory implementation seeded with a handful of notable birds.
/// </summary>
public class InMemoryBirdSanctuary : IBirdSanctuary
{
    private readonly List<Bird> _birds;
    private readonly List<BirdSighting> _sightings = new();

    public InMemoryBirdSanctuary()
    {
        _birds = SeedBirds();
        SeedSightings();
    }

    public IReadOnlyCollection<Bird> GetAllBirds() => _birds;

    public Bird? GetBirdByName(string name) =>
        _birds.FirstOrDefault(b =>
            string.Equals(b.Name, name, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyCollection<BirdSighting> GetRecentSightings(int max = 25) =>
        _sightings
            .OrderByDescending(s => s.ObservedAt)
            .Take(Math.Clamp(max, 1, 250))
            .ToArray();

    public BirdSighting AddSighting(string observer, string birdName, string location, string? notes)
    {
        var bird = GetBirdByName(birdName)
                   ?? throw new ArgumentException($"Unknown bird '{birdName}'.", nameof(birdName));

        var sighting = new BirdSighting
        {
            Observer = string.IsNullOrWhiteSpace(observer) ? "Anonymous birder" : observer.Trim(),
            Bird = bird,
            Location = location.Trim(),
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            ObservedAt = DateTimeOffset.UtcNow
        };

        _sightings.Add(sighting);
        return sighting;
    }

    private static List<Bird> SeedBirds() =>
        new()
        {
            new Bird
            {
                Name = "Peregrine Falcon",
                ScientificName = "Falco peregrinus",
                Habitat = "Cliffs, mountains, and increasingly cities",
                WingspanCm = 95,
                Diet = "Medium-sized birds caught on the wing",
                FunFact = "The peregrine is the fastest animal on earth, diving at over 320 km/h (200 mph).",
                Rarity = "Uncommon but recovering",
                Call = "kek-kek-kek!"
            },
            new Bird
            {
                Name = "Common Swift",
                ScientificName = "Apus apus",
                Habitat = "Towns, cities, and cliffs",
                WingspanCm = 40,
                Diet = "Flying insects",
                FunFact = "Swifts can stay airborne for months at a time, even sleeping on the wing.",
                Rarity = "Common",
                Call = "sriii-sriii!"
            },
            new Bird
            {
                Name = "Eurasian Blue Tit",
                ScientificName = "Cyanistes caeruleus",
                Habitat = "Woodlands, gardens, hedgerows",
                WingspanCm = 18,
                Diet = "Insects, seeds, and nuts",
                FunFact = "Blue tits are acrobatic feeders, often hanging upside-down from branches.",
                Rarity = "Common",
                Call = "tsirrr-tsirrr-chick!"
            },
            new Bird
            {
                Name = "Barn Owl",
                ScientificName = "Tyto alba",
                Habitat = "Open countryside, farmland, and woodland edges",
                WingspanCm = 95,
                Diet = "Small mammals, especially voles and mice",
                FunFact = "Barn owls can locate prey purely by sound, even in total darkness.",
                Rarity = "Uncommon",
                Call = "long eerie screech"
            },
            new Bird
            {
                Name = "Atlantic Puffin",
                ScientificName = "Fratercula arctica",
                Habitat = "North Atlantic sea cliffs and offshore islands",
                WingspanCm = 60,
                Diet = "Small fish such as sand eels",
                FunFact = "Puffins use their beaks like combs, carrying multiple fish crosswise at once.",
                Rarity = "Locally common, globally vulnerable",
                Call = "low grumbling growls"
            }
        };

    private void SeedSightings()
    {
        // A few sample sightings to make the sanctuary feel alive.
        AddSighting(
            observer: "EarlyLark",
            birdName: "Common Swift",
            location: "Old Town church tower",
            notes: "First swifts of the year circling high and screaming joyfully.");

        AddSighting(
            observer: "OwlEyes",
            birdName: "Barn Owl",
            location: "Meadow by the river",
            notes: "Silent ghost skimming over the grass just after sunset.");

        AddSighting(
            observer: "CityHawk",
            birdName: "Peregrine Falcon",
            location: "Downtown office block",
            notes: "Perched on the skyscraper ledge, then stooped toward the river.");
    }
}