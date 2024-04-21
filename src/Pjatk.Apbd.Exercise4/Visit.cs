namespace Pjatk.Apbd.Exercise4;

public record Visit(DateOnly Date, Guid AnimalId, string Description, decimal Price);

public record VisitWithId(Guid Id, DateOnly Date, Guid AnimalId, string Description, decimal Price)
    : Visit(Date, AnimalId, Description, Price)
{
    public VisitWithId(Guid Id, Visit visit)
        : this(Id, visit.Date, visit.AnimalId, visit.Description, visit.Price) { }
}
