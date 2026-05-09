using Model.Interfaces;

namespace Model.Core;

public class ResearchArticle : Article
{
    public string Methodology { get; private set; }
    public string Results { get; private set; }

    public ResearchArticle(string title, string text, string[] keyWords,
        List<Author> authors, DateTime publishedAt,
        string methodology = "", string results = "")
        : base(title, text, keyWords, authors, publishedAt, ArticleType.Research)
    {
        Methodology = methodology;
        Results = results;
    } 
    public ResearchArticle(){ }
}