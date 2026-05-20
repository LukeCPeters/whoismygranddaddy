using WhoIsMyGranddaddy.Core;
using Xunit;

namespace WhoIsMyGranddaddy.Tests;

public class GenealogyServiceTests
{
    private static GenealogyService BuildService() => new(new InMemoryPersonRepository(
    [
        Person(1, null, null, "Albert", "1900-01-01", "A1"),
        Person(2, null, null, "Bertha", "1905-01-01", "B2"),
        Person(3, 1, 2, "Carl", "1930-01-01", "C3"),
        Person(4, null, null, "Dora", "1928-01-01", "D4"),
        Person(5, 3, 4, "Erik", "1955-01-01", "E5"),
        Person(6, 3, 4, "Frida", "1958-01-01", "F6"),
        Person(8, null, null, "Hanna", "1979-01-01", "H8"),
        Person(7, 5, 8, "Greta", "1980-01-01", "G7"),
    ]));

    [Fact]
    public async Task Descendants_buildsTree_withCorrectLevels_andExcludesMarriedInSpouses()
    {
        var tree = await BuildService().GetDescendantsAsync("A1", maxDepth: 10);

        Assert.NotNull(tree.Root);
        Assert.False(tree.HasMore);
        Assert.Equal(new[] { 1, 3, 5, 6, 7 }, Ids(tree.Root!).Order());

        Assert.Equal(0, LevelOf(tree.Root!, 1));
        Assert.Equal(1, LevelOf(tree.Root!, 3));
        Assert.Equal(2, LevelOf(tree.Root!, 5));
        Assert.Equal(3, LevelOf(tree.Root!, 7));
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    public async Task Descendants_respectsMaxDepth_andReportsHasMore(int depth, bool expectedHasMore)
    {
        var tree = await BuildService().GetDescendantsAsync("A1", depth);

        Assert.Equal(expectedHasMore, tree.HasMore);
        Assert.All(Levels(tree.Root!), level => Assert.True(level <= depth));
    }

    [Fact]
    public async Task Descendants_attachCoParentsAsPartners()
    {
        var root = (await BuildService().GetDescendantsAsync("A1", maxDepth: 10)).Root!;
        var carl = root.Children.Single();

        Assert.Equal("Bertha", Assert.Single(root.Partners).Name);
        Assert.Equal("Dora", Assert.Single(carl.Partners).Name);
    }

    [Fact]
    public async Task RootAscendant_returnsOldestReachableAncestor()
    {
        var result = await BuildService().FindRootAscendantAsync("G7");

        Assert.NotNull(result.Start);
        Assert.Equal("Albert", result.Root!.Name);
    }

    [Fact]
    public async Task RootAscendant_ofPersonWithoutParents_isThemselves()
    {
        var result = await BuildService().FindRootAscendantAsync("A1");

        Assert.Equal(result.Start!.Id, result.Root!.Id);
    }

    [Fact]
    public async Task UnknownIdentityNumber_returnsEmptyResults()
    {
        var service = BuildService();

        Assert.Null((await service.GetDescendantsAsync("nope", 10)).Root);
        Assert.Null((await service.FindRootAscendantAsync("nope")).Start);
    }

    private static Person Person(int id, int? fatherId, int? motherId, string name, string birth, string idNo) =>
        new(id, fatherId, motherId, name, "Test", DateOnly.Parse(birth), idNo);

    private static IEnumerable<int> Ids(PersonNode node) =>
        new[] { node.Person.Id }.Concat(node.Children.SelectMany(Ids));

    private static IEnumerable<int> Levels(PersonNode node) =>
        new[] { node.Level }.Concat(node.Children.SelectMany(Levels));

    private static int LevelOf(PersonNode node, int id) =>
        node.Person.Id == id ? node.Level : node.Children.Select(c => LevelOf(c, id)).DefaultIfEmpty(-1).Max();
}
