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
    .WithName("GetAnimal")
    .Produces<AnimalWithId>()
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.MapDelete(
        "/animals/{Id}",
        async (Guid Id, IAnimalRepository repository) =>
        {
            await repository.Delete(Id);
            return Results.NoContent();
        }
    )
    .Produces(StatusCodes.Status204NoContent)
    .WithOpenApi();

app.MapPost(
        "/animals",
        async (Animal animal, IAnimalRepository repository) =>
        {
            var result = await repository.Add(animal);
            return Results.CreatedAtRoute("GetAnimal", new { Id = result.Id }, result);
        }
    )
    .Produces<AnimalWithId>(StatusCodes.Status201Created)
    .WithOpenApi();

app.MapPut(
        "/animals/{Id}",
        async (Guid Id, Animal animal, IAnimalRepository repository) =>
        {
            var animalWithId = new AnimalWithId(Id, animal);
            var result = await repository.AddOrUpdate(animalWithId);
            return result.Created
                ? Results.CreatedAtRoute("GetAnimal", new { Id = result.Animal.Id }, result.Animal)
                : Results.Ok(result.Animal);
        }
    )
    .Produces<AnimalWithId>(StatusCodes.Status200OK)
    .Produces<AnimalWithId>(StatusCodes.Status201Created)
    .WithOpenApi();

app.Run();
