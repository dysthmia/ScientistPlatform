using Model.Interfaces;

namespace Model.Core;

public static class CitationService
{
    private const string CitationsFolder = "citations";

    public static string CreateCitation(Article article, Publisher publisher)
    {
        ArgumentNullException.ThrowIfNull(article);
        ArgumentNullException.ThrowIfNull(publisher);

        if (article is not ICitation)
            throw new InvalidOperationException("Статья должна реализовывать ICitation.");

        return article.FormatCitation(publisher);
    }

    public static string SaveCitationToTxt(string citation, string fileName)
    {
        if (string.IsNullOrWhiteSpace(citation))
            throw new ArgumentException("Ссылка не может быть пустой.", nameof(citation));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла не может быть пустым.", nameof(fileName));

        Directory.CreateDirectory(CitationsFolder);

        var safeName = Path.GetInvalidFileNameChars()
            .Aggregate(fileName, (current, c) => current.Replace(c, '_'));

        if (!safeName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            safeName += ".txt";

        var fullPath = Path.Combine(CitationsFolder, safeName);
        File.WriteAllText(fullPath, citation);
        return fullPath;
    }
}
