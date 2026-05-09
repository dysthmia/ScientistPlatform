using Model.Core;
namespace Model.Interfaces;

public interface IArticle
{
    string Title { get; }
    string Text { get; }
    string[] Keywords { get; }
    DateTime PublishedAt { get; }
    string ISSN { get; }
    List<Author> Authors { get; }
    ArticleType Type { get; }
}

public enum ArticleType
{
    Research,
    Review,
    CaseStudy
}