using Dapper;
using Microsoft.Data.SqlClient;
using WhoIsMyGranddaddy.Core;

namespace WhoIsMyGranddaddy.Web.Data;

public sealed class SqlPersonRepository(string connectionString) : IPersonRepository
{
    private const string Columns = "Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber";

    public async Task<IReadOnlyList<DescendantRow>> GetDescendantsAsync(string identityNumber, int maxDepth)
    {
        const string sql = """
            WITH Tree AS (
                SELECT Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber, 0 AS Depth
                FROM site.Person
                WHERE IdentityNumber = @identityNumber
                UNION ALL
                SELECT c.Id, c.FatherId, c.MotherId, c.Name, c.Surname, c.BirthDate, c.IdentityNumber, t.Depth + 1
                FROM site.Person c
                INNER JOIN Tree t ON c.FatherId = t.Id OR c.MotherId = t.Id
                WHERE t.Depth < @maxDepth
            )
            SELECT Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber, Depth
            FROM Tree
            OPTION (MAXRECURSION 0);
            """;

        await using var connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<DescendantDto>(sql, new { identityNumber, maxDepth });
        return rows.Select(r => r.ToRow()).ToList();
    }

    public async Task<IReadOnlyList<Person>> GetAncestorsAsync(string identityNumber)
    {
        const string sql = """
            WITH Chain AS (
                SELECT Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber, 0 AS Depth
                FROM site.Person
                WHERE IdentityNumber = @identityNumber
                UNION ALL
                SELECT p.Id, p.FatherId, p.MotherId, p.Name, p.Surname, p.BirthDate, p.IdentityNumber, c.Depth + 1
                FROM site.Person p
                INNER JOIN Chain c ON p.Id = c.FatherId OR p.Id = c.MotherId
                WHERE c.Depth < 100
            )
            SELECT DISTINCT Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber
            FROM Chain
            OPTION (MAXRECURSION 0);
            """;

        await using var connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<Person>(sql, new { identityNumber });
        return rows.ToList();
    }

    public async Task<IReadOnlyList<Person>> GetByIdsAsync(IReadOnlyCollection<int> ids)
    {
        if (ids.Count == 0)
            return [];

        await using var connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<Person>(
            $"SELECT {Columns} FROM site.Person WHERE Id IN @ids",
            new { ids });
        return rows.ToList();
    }

    private sealed record DescendantDto(
        int Id, int? FatherId, int? MotherId, string Name, string Surname,
        DateOnly? BirthDate, string IdentityNumber, int Depth)
    {
        public DescendantRow ToRow() =>
            new(new Person(Id, FatherId, MotherId, Name, Surname, BirthDate, IdentityNumber), Depth);
    }
}
