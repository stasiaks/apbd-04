using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Pjatk.Apbd.Exercise4;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.NoContent())
    .Produces(StatusCodes.Status204NoContent)
    .WithOpenApi();

app.MapGet(
        "/animals",
        async (IAnimalRepository repository) =>
        {
            var result = await repository.GetAll();
            return Results.Ok(result);
        }
    )
    .Produces<IEnumerable<AnimalWithId>>()
    .WithOpenApi();

app.MapGet(
        "/animals/{Id}",
        async (Guid Id, IAnimalRepository repository) =>
        {
            var result = await repository.Get(Id);
            if (result is not null)
            {
                return Results.Ok(result);
            }
            return Results.NotFound();
        }
    )
    .Produces<AnimalWithId>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.Run();
