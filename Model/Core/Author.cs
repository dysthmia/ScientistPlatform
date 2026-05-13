using System.Runtime.Intrinsics.Arm;

namespace Model.Core;

public class Author
{
    public string Name { get; private set; }
    public string ORCID { get; private set; }
    public List<Article> Articles { get; private set; }

    public Author(string name)
    {
        Name = name;
        ORCID = GenerateORCID();
        Articles = new List<Article>();
    }

    private static string GenerateORCID()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }

    private bool HasArticle (string issn)
    {
        if (string.IsNullOrWhiteSpace(issn)) return false;
        return Articles.Any(a => a.ISSN == issn);
    }      
    public void AddArticle (Article article)
    {
        if (article == null || HasArticle(article.ISSN)) return;
        Articles.Add(article);
    }
    public void RemoveArticle (Article article) => 
        Articles.Remove(article);
}