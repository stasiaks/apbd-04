namespace Pjatk.Apbd.Exercise4
{
    public record Animal(
        string Name,
        AnimalCategory Category,
        decimal MassInKilogram,
        string CoatColor
    );

    public record AnimalWithId(
        Guid Id,
        string Name,
        AnimalCategory Category,
        decimal MassInKilogram,
        string CoatColor
    ) : Animal(Name, Category, MassInKilogram, CoatColor)
    {
        public AnimalWithId(Guid Id, Animal animal)
            : this(Id, animal.Name, animal.Category, animal.MassInKilogram, animal.CoatColor) { }
    };

    public enum AnimalCategory
    {
        Other,
        Cat,
        Dog,
        Fish,

        /// <summary>
        /// This is the catch all category for a host of fuzzy mammals, including ferrets, rabbits, guinea pigs, gerbils and many other cuddly creatures.
        /// </summary>
        SmallMammal,
        Bird,
        Reptile,
        Amphibian,
        Equine
    }
}
