using System.Collections.Concurrent;

namespace Pjatk.Apbd.Exercise4
{
    public interface IAnimalRepository
    {
        Task<IEnumerable<AnimalWithId>> GetAll();
        Task<AnimalWithId?> Get(Guid id);
        Task<AnimalWithId> Add(Animal animal);
        Task<AnimalWithId> AddOrUpdate(AnimalWithId animal);
        Task Delete();
    }

    internal class InMemoryAnimalRepository : IAnimalRepository
    {
        private readonly ConcurrentDictionary<Guid, Animal> dictionary = new();

        public Task<AnimalWithId> Add(Animal animal)
        {
            throw new NotImplementedException();
        }

        public Task<AnimalWithId> AddOrUpdate(AnimalWithId animal)
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
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
}
