using Hello_World.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Make validation errors a little more compact and readable.
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .Select(ms => new
                {
                    field = ms.Key,
                    messages = ms.Value!.Errors.Select(e => e.ErrorMessage)
                });

            return new BadRequestObjectResult(new
            {
                message = "Your bird report had a few ruffled feathers.",
                errors
            });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our bird sanctuary.
builder.Services.AddSingleton<IBirdSanctuary, InMemoryBirdSanctuary>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Birdsong Sanctuary API";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Birdsong Sanctuary v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// A tiny bird-themed root endpoint.
app.MapGet("/", () => new
{
    message = "Welcome to the Birdsong Sanctuary API.",
    hint = "Spread your wings at /swagger or glide to /api/birds to see the flock."
});

app.Run();