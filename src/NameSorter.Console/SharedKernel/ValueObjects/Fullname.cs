namespace NameSorter.Console.SharedKernel.ValueObjects;

public sealed record Fullname
{
    private Fullname(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);

        if (string.IsNullOrEmpty(fullName))
        {
            throw new ArgumentException("Fullname cannot be empty");
        }

        var parts = fullName.Trim().Split([' '], StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
            throw new ArgumentException("A name must contain at least one given name and a last name.");

        LastName = parts[^1];
        var givenNames = parts.Take(parts.Length - 1).ToArray();

        if (givenNames.Length is < 1 or > 3)
            throw new ArgumentException("A name must have at least 1 and at most 3 given names.");

        GivenName = string.Join(' ', givenNames);
    }

    public static Fullname Create(string fullName)
    {
        return new Fullname(fullName);
    }

    public string GivenName { get; init; }
    public string LastName { get; init; }

    public override string ToString()
    {
        return $"{GivenName} {LastName}";
    }
}