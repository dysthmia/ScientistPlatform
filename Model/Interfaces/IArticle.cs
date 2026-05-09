using Model.Core;

namespace Model.Interfaces;

public interface Article
{
    string Title { get; set; }
    string Text { get; set; }
    string[] Keywords { get; set; }
    DateTime PublishedAt { get; set; }
    string ISSN { get; set; }
    List<Author> Authors { get; set; }
    ArticleType Type { get; }
}

public enum ArticleType
{
    Research,
    Review,
    CaseStudy
}