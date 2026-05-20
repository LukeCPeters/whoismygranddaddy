namespace WhoIsMyGranddaddy.Core;

public record DescendantTree(PersonNode? Root, int MaxDepth, bool HasMore);

public record RootAscendantResult(Person? Start, Person? Root);

public sealed record DescendantRow(Person Person, int Depth);
