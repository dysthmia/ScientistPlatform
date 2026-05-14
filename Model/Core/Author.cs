using System.Runtime.Intrinsics.Arm;

namespace Model.Core;

public class Author
{
    private List<Article> _articles;

    public string Name { get; private set; }
    public string ORCID { get; private set; }
    public Article[] Articles  => _articles.ToArray();

    public Author(string name)
    {
        ValidateName(name);

        Name = name;
        ORCID = GenerateORCID();
        _articles = new List<Article>();
    }

    private static string GenerateORCID()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }

    public void AddArticle (Article article)
    {
       if (!CheckAvailabilityArticle(article)) _articles.Add(article);
    }
    public void RemoveArticle (Article article)
    {
        if (CheckAvailabilityArticle(article)) _articles.Remove(article);
    }
    
    private bool CheckAvailabilityArticle(Article article)
    {
        if (article == null)
            throw new ArgumentNullException(nameof(article), "Article null типа");
        
        if (string.IsNullOrWhiteSpace(article.ISSN))
            throw new ArgumentException(nameof(article), "ISSN не может быть пустым");
        
        return _articles.Any(a => a.ISSN == article.ISSN);
    }
    private void ValidateName (string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name), "name автора не может быть пустым");
    }
}