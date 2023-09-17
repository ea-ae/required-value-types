# Required Value Types

How do you deal with required value types in input models without having to make all of them nullable?

Implementation & sample controller are provided.

[Read the article/tutorial here.](http://localhost:3000/blog/clean-way-to-use-required-value-types-in-asp-net-core)

## How to use

Copy over the `RequiredValueTypes` folder to your project.

Then add the following line, somewhere after any other services that change `MvcOptions`:

```csharp
builder.Services.UseExplicitRequiredValueTypes();
```

Now you can use the `[Required]` attribute in your input models and it'll work with value types.

## Sample

```csharp
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
    public PersonInputModel Person { get; init; } = null!;
}

public class AppController : ControllerBase
{
    [Route("app")]
    public IActionResult App([FromBody] PersonAtLocationInputModel inputModel)
    {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        return Ok(inputModel);
    }
}
```

## License

MIT. See [LICENSE.md](LICENSE.md).
