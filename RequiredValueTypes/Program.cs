using RequiredValueTypes.RequiredValueTypes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.UseExplicitRequiredValueTypes();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
