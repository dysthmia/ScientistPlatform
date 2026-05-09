using Model.Interfaces;

namespace Model.Core;

public class CaseStudy : Article
{
    public string CaseDescription { get; private set; }
    public string Conclusions { get; private set; }

    public CaseStudy (string title,
                      string text, 
                      string[] keyWords,
                      List<Author> authors, 
                      DateTime publishedAt,
                      string caseDescription = "",
                      string conclusions = "")
        : base (title, text, keyWords, publishedAt, ArticleType.CaseStudy, authors)
    {
        CaseDescription = caseDescription;
        Conclusions = conclusions;
    } 
}