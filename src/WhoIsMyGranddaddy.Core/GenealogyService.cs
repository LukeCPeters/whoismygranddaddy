namespace WhoIsMyGranddaddy.Core;

public sealed class GenealogyService(IPersonRepository repository)
{
    public async Task<DescendantTree> GetDescendantsAsync(string identityNumber, int maxDepth)
    {
        var rows = await repository.GetDescendantsAsync(identityNumber, maxDepth + 1);
        if (rows.Count == 0)
            return new DescendantTree(null, maxDepth, false);

        var shallowest = new Dictionary<int, DescendantRow>();
        foreach (var row in rows)
            if (!shallowest.TryGetValue(row.Person.Id, out var seen) || row.Depth < seen.Depth)
                shallowest[row.Person.Id] = row;

        var hasMore = shallowest.Values.Any(r => r.Depth > maxDepth);

        var nodes = shallowest.Values
            .Where(r => r.Depth <= maxDepth)
            .ToDictionary(r => r.Person.Id, r => new PersonNode(r.Person, r.Depth));

        var partnerIds = new Dictionary<PersonNode, HashSet<int>>();

        foreach (var node in nodes.Values
                     .Where(n => n.Level > 0)
                     .OrderBy(n => n.Person.BirthDate ?? DateOnly.MaxValue)
                     .ThenBy(n => n.Person.Id))
        {
            var child = node.Person;
            var parent = ParentNode(child, nodes);
            if (parent is null)
                continue;

            parent.Children.Add(node);

            var coParentId = child.FatherId == parent.Person.Id ? child.MotherId : child.FatherId;
            if (coParentId is int id)
                Partners(partnerIds, parent).Add(id);
        }

        await ResolvePartnersAsync(partnerIds);

        var rootNode = nodes[shallowest.Values.First(r => r.Depth == 0).Person.Id];
        return new DescendantTree(rootNode, maxDepth, hasMore);
    }

    public async Task<RootAscendantResult> FindRootAscendantAsync(string identityNumber)
    {
        var people = await repository.GetAncestorsAsync(identityNumber);
        var start = people.FirstOrDefault(p => p.IdentityNumber == identityNumber);
        if (start is null)
            return new RootAscendantResult(null, null);

        var roots = people.Where(p => p is { FatherId: null, MotherId: null }).ToList();
        IEnumerable<Person> candidates = roots.Count > 0 ? roots : people;
        var oldest = candidates
            .OrderBy(p => p.BirthDate ?? DateOnly.MaxValue)
            .ThenBy(p => p.Id)
            .First();

        return new RootAscendantResult(start, oldest);
    }

    private async Task ResolvePartnersAsync(Dictionary<PersonNode, HashSet<int>> partnerIds)
    {
        if (partnerIds.Count == 0)
            return;

        var ids = partnerIds.Values.SelectMany(s => s).Distinct().ToList();
        var people = (await repository.GetByIdsAsync(ids)).ToDictionary(p => p.Id);

        foreach (var (node, set) in partnerIds)
            node.Partners.AddRange(set
                .Where(people.ContainsKey)
                .Select(id => people[id])
                .OrderBy(p => p.BirthDate ?? DateOnly.MaxValue)
                .ThenBy(p => p.Id));
    }

    private static PersonNode? ParentNode(Person child, IReadOnlyDictionary<int, PersonNode> nodes)
    {
        if (child.FatherId is int f && nodes.TryGetValue(f, out var father))
            return father;
        if (child.MotherId is int m && nodes.TryGetValue(m, out var mother))
            return mother;
        return null;
    }

    private static HashSet<int> Partners(Dictionary<PersonNode, HashSet<int>> map, PersonNode node)
    {
        if (!map.TryGetValue(node, out var set))
            map[node] = set = [];
        return set;
    }
}
