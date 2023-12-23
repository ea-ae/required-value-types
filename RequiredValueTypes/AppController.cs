using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace RequiredValueTypes;

public record PersonInputModel
{
    [Required(ErrorMessage = "Provide an ID for the user.")] public int Id { get; init; }
    public int? Age { get; init; }
    public string Name { get; init; } = null!;
}

public record PersonAtLocationInputModel
{
    [Required] public decimal Latitude { get; init; }
    [Required] public decimal Longitude { get; init; }
    [Required] public PersonInputModel Person { get; init; } = null!;
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
