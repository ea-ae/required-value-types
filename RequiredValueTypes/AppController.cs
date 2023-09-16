using Microsoft.AspNetCore.Mvc;

namespace RequiredValueTypes;

public record PersonInputModel
{
    public int Id { get; init; } // required
    public int? Age { get; init; } // optional
    public string Name { get; init; } = null!; // required
    public string? OptionalName { get; init; } // optional
}

public record PersonAtLocationInputModel
{
    public decimal Latitude { get; init; } // required
    public decimal Longitude { get; init; } // required
    public PersonInputModel Person { get; init; } = null!; // optional
}

[Route("app")]
public class AppController : ControllerBase
{
    [Route("query")]
    public IActionResult InputQuery(PersonAtLocationInputModel inputModel)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        return Ok(inputModel);
    }

    [Route("body")]
    public IActionResult InputBody([FromBody] PersonAtLocationInputModel inputModel)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        return Ok(inputModel);
    }
}
