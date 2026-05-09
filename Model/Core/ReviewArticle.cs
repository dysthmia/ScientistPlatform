using Model.Interfaces;

namespace Model.Core;

public class ReviewArticle : Article
{
    public string[] Sources { get; private set; }
    public string ReviewPeriod { get; private set; }

    public ReviewArticle(string title, string text, string[] keyWords,
        List<Author> authors, DateTime publishedAt,
        string[] sources = null, string reviewPeriod = "")
        : base(title, text, keyWords, authors, publishedAt, ArticleType.Review)
    {
        Sources = sources ?? Array.Empty<string>();
        ReviewPeriod = reviewPeriod;
    } 
    public ReviewArticle(){ }
}