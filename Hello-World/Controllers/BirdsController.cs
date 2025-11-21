using System;
using System.Collections.Generic;
using System.Linq;
using Hello_World.Models;
using Hello_World.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hello_World.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BirdsController : ControllerBase
{
    private readonly IBirdSanctuary _sanctuary;

    public BirdsController(IBirdSanctuary sanctuary)
    {
        _sanctuary = sanctuary;
    }

    /// <summary>
    /// List all birds currently in the sanctuary.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Bird>> GetAllBirds()
        => Ok(_sanctuary.GetAllBirds());

    /// <summary>
    /// Get a single bird by its common name.
    /// </summary>
    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Bird> GetBirdByName(string name)
    {
        var bird = _sanctuary.GetBirdByName(name);
        if (bird is null)
        {
            return NotFound(new { message = $"No bird named '{name}' is roosting in this sanctuary." });
        }

        return Ok(bird);
    }

    /// <summary>
    /// Get a simple bird call by name.
    /// </summary>
    [HttpPost("call")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BirdCallResponse> GetBirdCall([FromBody] BirdCallRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var bird = _sanctuary.GetBirdByName(request.Name);
        if (bird is null)
        {
            return NotFound(new { message = $"The winds carry no song from a bird named '{request.Name}'." });
        }

        var response = new BirdCallResponse
        {
            Name = bird.Name,
            Call = bird.Call,
            Description = request.IncludeDescription
                ? $"{bird.Name} typically lives in {bird.Habitat.ToLowerInvariant()} and feeds on {bird.Diet.ToLowerInvariant()}."
                : null
        };

        return Ok(response);
    }

    /// <summary>
    /// Get recent bird sightings recorded in the sanctuary.
    /// </summary>
    [HttpGet("sightings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<BirdSighting>> GetSightings([FromQuery] int max = 25)
    {
        max = Math.Clamp(max, 1, 250);
        var sightings = _sanctuary.GetRecentSightings(max);
        return Ok(sightings);
    }

    /// <summary>
    /// Record a new bird sighting.
    /// </summary>
    [HttpPost("sightings")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BirdSighting> AddSighting([FromBody] AddSightingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var sighting = _sanctuary.AddSighting(
                observer: request.Observer ?? "Anonymous birder",
                birdName: request.BirdName,
                location: request.Location,
                notes: request.Notes);

            return CreatedAtAction(
                nameof(GetSightings),
                new { id = sighting.Id },
                sighting);
        }
        catch (ArgumentException ex) when (ex.ParamName == "birdName")
        {
            return NotFound(new { message = ex.Message });
        }
    }

    public class AddSightingRequest
    {
        /// <summary>
        /// Optional nickname of the observer.
        /// </summary>
        public string? Observer { get; set; }

        /// <summary>
        /// Common name of the bird that was seen.
        /// </summary>
        public string BirdName { get; set; } = string.Empty;

        /// <summary>
        /// Rough location description.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Optional notes about the observation.
        /// </summary>
        public string? Notes { get; set; }
    }
}