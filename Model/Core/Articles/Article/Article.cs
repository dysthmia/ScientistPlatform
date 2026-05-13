using Model.Interfaces;
using Newtonsoft.Json;
namespace Model.Core;

public partial class Article : IArticle
{
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string[] KeyWords { get; private set; }
    public DateTime PublishedAt { get; private set; }
    public string ISSN { get; private set; }
    public List<Author> Authors { get; private set; }
    public ArticleType Type { get; private set; }

    public Article (string title, 
                      string text, 
                      string[] keywords, 
                      DateTime publishedAt, 
                      ArticleType type,
                      List<Author> authors)
    {
        if (authors == null || authors.Count > 10) return;
        
        Title = title;
        Text = text;
        KeyWords = keywords;
        PublishedAt = publishedAt;
        ISSN = GenerateISSN();
        Authors = authors;
        Type = type;
    }
    
    [JsonConstructor]
    protected Article(string title,
                    string text,
                    string[] keywords,
                    DateTime publishedAt,
                    ArticleType type,
                    List<Author> authors,
                    string issn = "")
    {
        Title = title;
        Text = text;
        KeyWords = keywords;
        PublishedAt = publishedAt;
        ISSN = string.IsNullOrWhiteSpace(issn) ? GenerateISSN() : issn;
        Authors = authors;
        Type = type;
    }

    private static string GenerateISSN()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }

    public bool HasKeyWords (params string[] keywords)
    {
        if (keywords == null || keywords.Length == 0) return false;
        foreach (var keyword in keywords)
        {
            if (KeyWords.Any(k => k == keyword)) return true;
        }
        return false;
    }
    
    private bool HasAuthor(string orcid)
    {
        if (string.IsNullOrWhiteSpace(orcid)) return false;
        return Authors.Any(a => a.ORCID == orcid);
    }
    public void AddAuthor (Author author)
    {
        if (author == null || HasAuthor(author.ORCID)) return;
        Authors.Add(author);
    }
    public void RemoveAuthor (Author author) =>
        Authors.Remove(author);
}

