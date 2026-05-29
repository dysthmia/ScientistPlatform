using Model.Interfaces;
using Newtonsoft.Json;
namespace Model.Core;

public class CaseStudy : Article
{
    public string CaseDescription { get; private set; }
    public string Conclusions { get; private set; }

    public CaseStudy (string title,
                      string text, 
                      string[] keyWords,
                      List<Author> authors, 
                      string caseDescription = "",
                      string conclusions = "")
        : base (title, text, keyWords, ArticleType.CaseStudy, authors)
    {
        CaseDescription = caseDescription;
        Conclusions = conclusions;
    } 
    
    [JsonConstructor]
    public CaseStudy(string title,
                     string text,
                     string[] keywords,
                     List<Author> authors,
                     DateTime publishedAt,
                     string caseDescription = "",
                     string conclusions = "",
                     string issn = "",
                     Publisher? publisher = null)
        : base(title, text, keywords, publishedAt, ArticleType.CaseStudy, authors, issn)
    {
        CaseDescription = caseDescription;
        Conclusions = conclusions;

        if (publisher != null)
            AddPublisher(publisher);
    }
}
