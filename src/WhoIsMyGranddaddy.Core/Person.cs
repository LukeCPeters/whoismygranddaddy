namespace WhoIsMyGranddaddy.Core;

public record Person(
    int Id,
    int? FatherId,
    int? MotherId,
    string Name,
    string Surname,
    DateOnly? BirthDate,
    string IdentityNumber)
{
    public string FullName => $"{Name} {Surname}";
}
