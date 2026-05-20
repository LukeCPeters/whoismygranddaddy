using WhoIsMyGranddaddy.Core;

namespace WhoIsMyGranddaddy.Web.Models;

public sealed class AncestorViewModel
{
    public Person? Start { get; init; }
    public Person? Root { get; init; }
    public string? Notice { get; init; }
}
