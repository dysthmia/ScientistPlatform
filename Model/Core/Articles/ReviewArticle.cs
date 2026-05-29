using Model.Interfaces;
using Newtonsoft.Json;
namespace Model.Core;

public class ReviewArticle : Article
{
    public string[] Sources { get; private set; }
    public string ReviewPeriod { get; private set; }

    public ReviewArticle (string title, 
                          string text, 
                          string[] keyWords,
                          List<Author> authors,
                          string[] sources = null!, 
                          string reviewPeriod = "")
        : base(title, text, keyWords, ArticleType.Review, authors)
    {
        Sources = sources ?? Array.Empty<string>();
        ReviewPeriod = reviewPeriod;
    } 
    
    [JsonConstructor]
    public ReviewArticle(string title,
                         string text,
                         string[] keywords,
                         DateTime publishedAt,
                         List<Author> authors,
                         string[] sources = null!,
                         string reviewPeriod = "",
                         string issn = "",
                         Publisher? publisher = null)
        : base(title, text, keywords, publishedAt, ArticleType.Review, authors, issn)
    {
        Sources = sources ?? Array.Empty<string>();
        ReviewPeriod = reviewPeriod;

        if (publisher != null)
            AddPublisher(publisher);
    }
}
