using Model.Core;
namespace Model.Interfaces;

public interface IArticle
{
    string Title { get; }
    string Text { get; }
    string[] KeyWords { get; }
    DateTime PublishedAt { get; }
    string ISSN { get; }
    Author[] Authors { get; }
    ArticleType Type { get; }
}

public enum ArticleType
{
    Research,
    Review,
    CaseStudy
}