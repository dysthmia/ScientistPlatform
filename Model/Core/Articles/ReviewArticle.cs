using Model.Interfaces;

namespace Model.Core;

public class ReviewArticle : Article
{
    public string[] Sources { get; private set; }
    public string ReviewPeriod { get; private set; }

    public ReviewArticle (string title, 
                          string text, 
                          string[] keyWords,
                          DateTime publishedAt,
                          List<Author> authors,
                          string[] sources = null!, 
                          string reviewPeriod = "")
        : base(title, text, keyWords, publishedAt, ArticleType.Review, authors)
    {
        Sources = sources ?? Array.Empty<string>();
        ReviewPeriod = reviewPeriod;
    } 
}