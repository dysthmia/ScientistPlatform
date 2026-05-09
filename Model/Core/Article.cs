using Model.Interfaces;

namespace Model.Core;

public partial class Article : IArticle
{
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string[] Keywords { get; private set; }
    public DateTime PublishedAt { get; private set; }
    public string ISSN { get; private set; }
    public List<Author> Authors { get; private set; }
    public ArticleType Type { get; private set; }

    protected Article(string title, string text, string[] keywords, DateTime publishedAt, ArticleType type)
    {
        Title = title;
        Text = text;
        Keywords = keywords;
        PublishedAt = publishedAt;
        ISSN = GenerateISSN();
        Authors = new List<Author>();
        Type = type;
    }
    protected Article(){ }

    private static string GenerateISSN()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }
}

public partial class Article
{
    public Publisher Publisher { get; private set; }

    public bool AddPublisher(Publisher publisher)
    {
        if (publisher == null) return false;

        bool mathcesTheme = Keywords.Any(Keywords => publisher.Themes.Any(theme => theme.Equals(Keywords, StringComparison.OrdinalIgnoreCase)));

        if (!mathcesTheme) return false;

        Publisher = publisher;
        return true;
    }

    public void RemovePublisher()
    {
        Publisher = null;
    }
}

public partial class Article : ICitation
{
    //
}