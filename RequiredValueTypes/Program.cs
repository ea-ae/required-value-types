using RequiredValueTypes.RequiredValueTypes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.UseRequiredValueTypes();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.Run();
