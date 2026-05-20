namespace WhoIsMyGranddaddy.Core;

public sealed class PersonNode(Person person, int level)
{
    public Person Person { get; } = person;
    public int Level { get; } = level;
    public List<PersonNode> Children { get; } = [];

    public List<Person> Partners { get; } = [];
}
