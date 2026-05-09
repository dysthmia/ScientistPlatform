using Model.Interfaces;
namespace Model.Core.DTOs;

public class DtoArticle
{
    string Title { get; set; }
    string Text { get; set; }
    string[] Keywords { get; set; }
    DateTime PublishedAt { get; set; }
    string ISSN { get; set; }
    List<Author> Authors { get; set; }
    ArticleType Type { get; set; }
}