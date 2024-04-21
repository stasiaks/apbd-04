using System.Collections.Concurrent;

namespace Pjatk.Apbd.Exercise4;

public interface IVisitRepository
{
    Task<IEnumerable<VisitWithId>> GetAllForAnimal(Guid animalId);
    Task<VisitWithId?> Get(Guid id);
    Task<VisitWithId> Add(Visit visit);
}

internal class InMemoryVisitRepository : IVisitRepository
{
    private readonly ConcurrentDictionary<Guid, Visit> dictionary = new();

    public Task<VisitWithId> Add(Visit visit)
    {
        var newId = Guid.NewGuid();
        dictionary.GetOrAdd(newId, visit); // Nie obsługuję kolizji GUIDów

        return Task.FromResult(new VisitWithId(newId, visit));
    }

    public Task<VisitWithId?> Get(Guid id)
    {
        var visit = dictionary.GetValueOrDefault(id);
        VisitWithId? result = visit is null ? null : new(id, visit);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<VisitWithId>> GetAllForAnimal(Guid animalId)
    {
        var results = dictionary
            .Where(x => x.Value.AnimalId == animalId)
            .Select(x => new VisitWithId(x.Key, x.Value));
        return Task.FromResult(results);
    }
}
