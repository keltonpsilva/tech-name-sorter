using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console.Features.SortName;

public interface INameParser
{
    IReadOnlyList<Fullname> Parse(HashSet<string> names);
}

public sealed class NameParserService : INameParser
{
    public IReadOnlyList<Fullname> Parse(HashSet<string> names)
    {
        List<Fullname> fullnames = [];

        fullnames.AddRange(names.Select(Fullname.Create));

        return fullnames;
    }
}