using Model.Interfaces;
using Newtonsoft.Json;
namespace Model.Core;

public class ResearchArticle : Article
{
    public string Methodology { get; private set; }
    public string Results { get; private set; }

    public ResearchArticle (string title, 
                            string text, 
                            string[] keyWords,
                            List<Author> authors,
                            DateTime publishedAt,
                            string methodology = "", 
                            string results = "")
        : base(title, text, keyWords, publishedAt, ArticleType.Research, authors)
    {
        Methodology = methodology;
        Results = results;
    } 
    
    [JsonConstructor]
    public ResearchArticle(string title,
                           string text,
                           string[] keywords,
                           List<Author> authors,
                           DateTime publishedAt,
                           string methodology = "",
                           string results = "",
                           string issn = "")
        : base(title, text, keywords, publishedAt, ArticleType.Research, authors, issn)
    {
        Methodology = methodology;
        Results = results;
    }
}