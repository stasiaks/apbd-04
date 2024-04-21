namespace Pjatk.Apbd.Exercise2.Core;

// Jednostki SI implementowane jako osobone typy, celem uniknięcia "Primitive Obsession"
// Niestety C# nie wspiera "Units of Measure" https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/units-of-measure

public record Kilogram(decimal Value)
{
    public static explicit operator Kilogram(decimal value) => new(value);

    public override string ToString() => $"{Value} kg";
};
