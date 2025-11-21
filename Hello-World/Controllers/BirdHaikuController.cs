using Microsoft.AspNetCore.Mvc;

namespace Hello_World.Controllers;

[ApiController]
[Route("api/bird-haiku")]
public class BirdHaikuController : ControllerBase
{
    /// <summary>
    /// Returns a simple, procedurally generated bird-themed haiku.
    /// This endpoint exists purely for fun and to emphasize the bird theme.
    /// </summary>
    [HttpGet]
    public IActionResult GetHaiku()
    {
        // Very lightweight randomization without external dependencies.
        var timeSeed = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
        var random = new Random(timeSeed);

        string[] firstLines =
        {
            "Feathers catch dawn light",
            "Over quiet lakes",
            "City ledge high up",
            "Wind over pine trees",
            "Shoreline cliffs awake"
        };

        string[] secondLines =
        {
            "Swifts carve poems into sky",
            "Falcon shadows tilt and dive",
            "Owls stitch silence through the dark",
            "Puffins carry silver glints",
            "Small blue tit inspects each leaf"
        };

        string[] thirdLines =
        {
            "Sky remembers wings",
            "River keeps their songs",
            "Night folds into down",
            "Sea spray tastes of flight",
            "Nest waits, warm with dreams"
        };

        var haiku = new[]
        {
            firstLines[random.Next(firstLines.Length)],
            secondLines[random.Next(secondLines.Length)],
            thirdLines[random.Next(thirdLines.Length)]
        };

        return Ok(new
        {
            title = "Birdsong Haiku",
            lines = haiku
        });
    }
}