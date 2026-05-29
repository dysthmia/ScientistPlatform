using System.Globalization;
using Model.Core;

namespace ScientistPlatform.Views;

public class ArticleListItem
{
    private static readonly CultureInfo RussianCulture = CultureInfo.GetCultureInfo("ru-RU");

    public ArticleListItem(Article article)
    {
        Article = article;
        Title = article.Title;
        PublishedAt = article.PublishedAt.ToString("dd MMMM yyyy", RussianCulture);
        Authors = article.JoinAuthors();
    }

    public Article Article { get; }
    public string Title { get; }
    public string PublishedAt { get; }
    public string Authors { get; }
}