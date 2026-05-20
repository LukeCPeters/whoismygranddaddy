namespace WhoIsMyGranddaddy.Core;

public interface IPersonRepository
{
    Task<IReadOnlyList<DescendantRow>> GetDescendantsAsync(string identityNumber, int maxDepth);

    Task<IReadOnlyList<Person>> GetAncestorsAsync(string identityNumber);

    Task<IReadOnlyList<Person>> GetByIdsAsync(IReadOnlyCollection<int> ids);
}
