using System.Collections.Concurrent;

namespace Pjatk.Apbd.Exercise4;

public interface IAnimalRepository
{
    Task<IEnumerable<AnimalWithId>> GetAll();
    Task<AnimalWithId?> Get(Guid id);
    Task<AnimalWithId> Add(Animal animal);
    Task<(AnimalWithId Animal, bool Created)> AddOrUpdate(AnimalWithId animal);
    Task Delete(Guid id);
}

internal class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly ConcurrentDictionary<Guid, Animal> dictionary = new();

    public Task<AnimalWithId> Add(Animal animal)
    {
        var newId = Guid.NewGuid();
        dictionary.GetOrAdd(newId, animal); // Nie obsługuję kolizji GUIDów

        return Task.FromResult(new AnimalWithId(newId, animal));
    }

    public Task<(AnimalWithId Animal, bool Created)> AddOrUpdate(AnimalWithId animal)
    {
        var created = !dictionary.ContainsKey(animal.Id);
        dictionary.AddOrUpdate(animal.Id, _ => animal, (_, _) => animal);
        return Task.FromResult((animal, created));
    }

    public Task Delete(Guid id)
    {
        dictionary.TryRemove(id, out var _);
        return Task.CompletedTask;
    }

    public Task<AnimalWithId?> Get(Guid id)
    {
        var animal = dictionary.GetValueOrDefault(id);
        AnimalWithId? result = animal is null ? null : new(id, animal);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<AnimalWithId>> GetAll()
    {
        var results = dictionary.Select(x => new AnimalWithId(x.Key, x.Value));
        return Task.FromResult(results);
    }
}
