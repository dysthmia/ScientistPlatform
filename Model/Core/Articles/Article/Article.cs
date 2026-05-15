using Model.Interfaces;
using Newtonsoft.Json;
namespace Model.Core;

public abstract partial  class Article : IArticle
{
    private const int MaxAuthorsCount = 10;

    private List<Author> _authors;
    private string[] _keywords;

    public string Text { get; private set; }
    public string ISSN { get; private set; }
    public string Title { get; private set; }
    public ArticleType Type { get; private set; }
    public DateTime PublishedAt { get; private set; }

    public string[] KeyWords => _keywords.ToArray();
    public Author[] Authors => _authors.ToArray();

    public Article  (string title, 
                    string text, 
                    string[] keywords, 
                    ArticleType type,
                    List<Author> authors)
    {
        ValidateTitle(title);
        ValidateText(text);

        _keywords = ValidateAndCopyKeywords(keywords);

        _authors = new List<Author>();
        AddAuthor(authors);
        
        Title = title;
        Text = text;
        Type = type;

        PublishedAt = DateTime.Now;
        ISSN = GenerateISSN();
    }
    
    [JsonConstructor]
    protected Article   (string title,
                        string text,
                        string[] keywords,
                        DateTime publishedAt,
                        ArticleType type,
                        List<Author> authors,
                        string issn = "")
    {
        ValidateTitle(title);
        ValidateText(text);
        ValidatePublishedAt(publishedAt);

        _keywords = ValidateAndCopyKeywords(keywords);
        _authors = new List<Author>();
        AddAuthor(authors);

        Title = title;
        Text = text;
        Type = type;
        
        PublishedAt = publishedAt;
        ISSN = string.IsNullOrWhiteSpace(issn) ? GenerateISSN() : issn;
    }

    private static string GenerateISSN()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }

    public bool HasKeyWords(params string[] keywords)
    {
        string[] checkedKeywords = ValidateAndCopyKeywords(keywords);
        return checkedKeywords.Any(keyword =>
            _keywords.Contains(keyword, StringComparer.OrdinalIgnoreCase));
    }
    public void AddKeyWords(params string[] keywords)
    {
        string[] newKeywords = ValidateAndCopyNewKeywords(keywords);
        if (newKeywords.Length == 0) return;
        _keywords = _keywords.Concat(newKeywords).ToArray();
    }
    public void AddAuthor(Author author)
    {
        ValidateAuthor(author);
        if (HasAuthor(author)) return;
        if (_authors.Count >= MaxAuthorsCount)
            throw new InvalidOperationException($"Авторов не может быть больше {MaxAuthorsCount}.");

        _authors.Add(author);
    }
    public void AddAuthor (List<Author> authors)
    {
        foreach (var author in authors) AddAuthor(author);
    }
    public void RemoveAuthor(Author author)
    {
        ValidateAuthor(author);
        _authors.Remove(author);
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title не может быть пустым.", nameof(title));
    }
    private void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text не может быть пустым.", nameof(text));
    }
    private static string[] ValidateAndCopyKeywords(string[] keywords)
    {
        if (keywords == null)
            throw new ArgumentNullException(nameof(keywords), "Keywords null типа.");

        if (keywords.Length == 0)
            throw new ArgumentException("Keywords не может быть пустым.", nameof(keywords));

        if (keywords.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException(
                "Keywords не должен содержать null, пустые строки или пробелы.",
                nameof(keywords));

        return keywords
            .Select(keyword => keyword.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
    private string[] ValidateAndCopyNewKeywords(string[] keywords)
    {
        string[] checkedKeywords = ValidateAndCopyKeywords(keywords);

        return checkedKeywords
            .Where(keyword => !_keywords.Contains(keyword, StringComparer.OrdinalIgnoreCase))
            .ToArray();
    }
    private void ValidatePublishedAt(DateTime publishedAt)
    {
        if (publishedAt > DateTime.Now)
            throw new ArgumentOutOfRangeException(nameof(publishedAt),"Дата публикации не может быть в будущем.");
    }
    private static void ValidateAuthor(Author author)
    {
        if (author == null)
            throw new ArgumentNullException(nameof(author), "Author null типа.");

        if (string.IsNullOrWhiteSpace(author.ORCID))
            throw new ArgumentException("ORCID автора не может быть пустым.", nameof(author));
    }
    private bool HasAuthor(Author author) =>
        _authors.Any(a => a.ORCID == author.ORCID);
    
}

