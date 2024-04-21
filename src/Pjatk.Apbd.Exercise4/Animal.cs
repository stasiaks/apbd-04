using Pjatk.Apbd.Exercise2.Core;

public record Animal(Guid Id, string Name, AnimalCategory Category, Kilogram Kilogram);

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
