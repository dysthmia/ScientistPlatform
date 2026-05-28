using Model.Interfaces;

namespace Model.Core;

public abstract partial class Article : ICitation
{
    public string Citation { get; private set; } = string.Empty;

    public string FormatCitation(Publisher publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);

        Citation = publisher.CitationStyle switch
        {
            PublisherCitationStyle.Apa => FormatApa(publisher),
            PublisherCitationStyle.Gost => FormatGost(publisher),
            PublisherCitationStyle.Ieee => FormatIeee(publisher),
            _ => FormatApa(publisher)
        };

        return Citation;
    }

    public string FormatDefaultCitation()
    {
        Citation = $"{JoinAuthors()}. {Title}. {PublishedAt:yyyy}. ISSN {ISSN}.";
        return Citation;
    }

    private string FormatApa(Publisher publisher)
    {
        var authors = JoinAuthorsForApa();
        return $"{authors} ({PublishedAt:yyyy}). {Title}. {publisher.Name}. ISSN {ISSN}.";
    }

    private string FormatGost(Publisher publisher)
    {
        var authors = JoinAuthorsForGost();
        return $"{authors} {Title} // {publisher.Name}. – {PublishedAt:yyyy}. – ISSN {ISSN}.";
    }

    private string FormatIeee(Publisher publisher)
    {
        var authors = JoinAuthorsForIeee();
        return $"{authors}, \"{Title},\" {publisher.Name}, {PublishedAt:yyyy}, ISSN: {ISSN}.";
    }

    public string JoinAuthors() =>
        string.Join(", ", Authors.Select(a => a.Name));

    private string JoinAuthorsForApa()
    {
        var names = Authors.Select(a => a.Name).ToArray();

        return names.Length switch
        {
            0 => string.Empty,
            1 => names[0],
            2 => $"{names[0]} & {names[1]}",
            _ => $"{string.Join(", ", names[..^1])} & {names[^1]}"
        };
    }

    private string JoinAuthorsForGost() =>
        string.Join(", ", Authors.Select(a => FormatNameForGost(a.Name)));

    private string JoinAuthorsForIeee()
    {
        var names = Authors.Select(a => FormatNameForIeee(a.Name)).ToArray();

        return names.Length switch
        {
            0 => string.Empty,
            1 => names[0],
            2 => $"{names[0]} and {names[1]}",
            _ => $"{string.Join(", ", names[..^1])} and {names[^1]}"
        };
    }

    private static string FormatNameForGost(string fullName)
    {
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return fullName;
        if (parts.Length == 1) return parts[0];

        var surname = parts[0];
        var initials = string.Concat(parts.Skip(1).Select(p => $"{char.ToUpper(p[0])}."));
        return $"{surname} {initials}";
    }

    private static string FormatNameForIeee(string fullName)
    {
        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return fullName;
        if (parts.Length == 1) return parts[0];

        var surname = parts[0];
        var initials = string.Concat(parts.Skip(1).Select(p => $"{char.ToUpper(p[0])}."));
        return $"{initials} {surname}";
    }
}
