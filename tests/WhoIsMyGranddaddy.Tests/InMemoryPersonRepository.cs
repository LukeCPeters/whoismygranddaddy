using WhoIsMyGranddaddy.Core;

namespace WhoIsMyGranddaddy.Tests;

public sealed class InMemoryPersonRepository(IEnumerable<Person> people) : IPersonRepository
{
    private readonly List<Person> _people = people.ToList();

    public Task<IReadOnlyList<DescendantRow>> GetDescendantsAsync(string identityNumber, int maxDepth)
    {
        var root = _people.FirstOrDefault(p => p.IdentityNumber == identityNumber);
        if (root is null)
            return Task.FromResult<IReadOnlyList<DescendantRow>>([]);

        var rows = new List<DescendantRow>();
        var seen = new HashSet<int>();
        var frontier = new List<Person> { root };

        for (var depth = 0; frontier.Count > 0 && depth <= maxDepth; depth++)
        {
            foreach (var person in frontier)
                if (seen.Add(person.Id))
                    rows.Add(new DescendantRow(person, depth));

            if (depth == maxDepth)
                break;

            var parentIds = frontier.Select(p => p.Id).ToHashSet();
            frontier = _people
                .Where(c => (c.FatherId is int f && parentIds.Contains(f))
                            || (c.MotherId is int m && parentIds.Contains(m)))
                .ToList();
        }

        return Task.FromResult<IReadOnlyList<DescendantRow>>(rows);
    }

    public Task<IReadOnlyList<Person>> GetAncestorsAsync(string identityNumber)
    {
        var start = _people.FirstOrDefault(p => p.IdentityNumber == identityNumber);
        if (start is null)
            return Task.FromResult<IReadOnlyList<Person>>([]);

        var byId = _people.ToDictionary(p => p.Id);
        var found = new Dictionary<int, Person>();
        var pending = new Stack<Person>([start]);

        while (pending.Count > 0)
        {
            var person = pending.Pop();
            if (!found.TryAdd(person.Id, person))
                continue;
            if (person.FatherId is int f && byId.TryGetValue(f, out var father))
                pending.Push(father);
            if (person.MotherId is int m && byId.TryGetValue(m, out var mother))
                pending.Push(mother);
        }

        return Task.FromResult<IReadOnlyList<Person>>(found.Values.ToList());
    }

    public Task<IReadOnlyList<Person>> GetByIdsAsync(IReadOnlyCollection<int> ids)
    {
        var set = ids.ToHashSet();
        IReadOnlyList<Person> matches = _people.Where(p => set.Contains(p.Id)).ToList();
        return Task.FromResult(matches);
    }
}
