using WhoIsMyGranddaddy.Core;

namespace WhoIsMyGranddaddy.Web.Models;

public sealed class TreeViewModel
{
    public required string IdentityNumber { get; init; }
    public int MaxDepth { get; init; }
    public PersonNode? Root { get; init; }
    public bool HasMore { get; init; }
    public string? Notice { get; init; }
    public int NextDepth => MaxDepth + 10;
}
