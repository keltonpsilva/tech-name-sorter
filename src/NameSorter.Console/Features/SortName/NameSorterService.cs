using NameSorter.Console.SharedKernel.ValueObjects;

namespace NameSorter.Console.Features.SortName;

public interface INameSorter
{
    IReadOnlyList<Fullname> Sort(List<Fullname> names);
}

public sealed class NameSorterService : INameSorter
{
    public IReadOnlyList<Fullname> Sort(List<Fullname> names)
    {
        return names
            .OrderBy(n => n.LastName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(n => n.GivenName, StringComparer.OrdinalIgnoreCase).ToList();
    }
}