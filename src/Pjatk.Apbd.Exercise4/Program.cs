using Pjatk.Apbd.Exercise4;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
builder.Services.AddSingleton<IVisitRepository, InMemoryVisitRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Health

app.MapGet("/health", () => Results.NoContent())
    .Produces(StatusCodes.Status204NoContent)
    .WithTags("Health")
    .WithOpenApi();

#endregion

#region Animal

app.MapGet(
        "/animals",
        async (IAnimalRepository repository) =>
        {
            var result = await repository.GetAll();
            return Results.Ok(result);
        }
    )
    .Produces<IEnumerable<AnimalWithId>>()
    .WithTags("Animals")
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
    .WithTags("Animals")
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
    .WithTags("Animals")
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
    .WithTags("Animals")
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
    .WithTags("Animals")
    .WithOpenApi();

#endregion

app.MapGet(
        "/visits/{Id}",
        async (Guid Id, IVisitRepository repository) =>
        {
            var result = await repository.Get(Id);
            if (result is not null)
            {
                return Results.Ok(result);
            }
            return Results.NotFound();
        }
    )
    .WithName("GetVisit")
    .Produces<VisitWithId>()
    .Produces(StatusCodes.Status404NotFound)
    .WithTags("Visits")
    .WithOpenApi();

app.MapGet(
        "/visits",
        async (Guid animalId, IVisitRepository repository) =>
        {
            var result = await repository.GetAllForAnimal(animalId);
            return Results.Ok(result);
        }
    )
    .Produces<IEnumerable<VisitWithId>>()
    .WithTags("Visits")
    .WithOpenApi();

app.MapPost(
        "/visits",
        async (Visit visit, IVisitRepository repository, IAnimalRepository animalRepository) =>
        {
            var animal = await animalRepository.Get(visit.AnimalId);
            if (animal is null)
            {
                return Results.BadRequest($"Animal with id {visit.AnimalId} doesn't exist");
            }
            var result = await repository.Add(visit);
            return Results.CreatedAtRoute("GetVisit", new { Id = result.Id }, result);
        }
    )
    .Produces<VisitWithId>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithTags("Visits")
    .WithOpenApi();

app.Run();
