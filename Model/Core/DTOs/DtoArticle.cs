using Model.Interfaces;
namespace Model.Core.DTOs;

public class DtoArticle
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string[] Keywords { get; set; }
    public DateTime PublishedAt { get; set; }
    public string ISSN { get; set; }
    public string[] AuthorsNames { get; set; }
    public ArticleType Type { get; set; }

    // ResearchArticle
    public string Methodology { get; set; }
    public string Results { get; set; }

    // ReviewArticle
    public string[] Sources { get; set; }
    public string ReviewPeriod { get; set; }

    // CaseStudy
    public string CaseDescription { get; set; }
    public string Conclusions { get; set; }

    public DtoArticle()
    {
    }
    public DtoArticle(Article article)
    {
        Title = article.Title;
        Text = article.Text;
        Keywords = article.KeyWords;
        PublishedAt = article.PublishedAt;
        ISSN = article.ISSN;
        AuthorsNames = article.Authors.Select(a => a.Name).ToArray();
        Type = article.Type;

        Methodology = "";
        Results = "";

        Sources = Array.Empty<string>();
        ReviewPeriod = "";

        CaseDescription = "";
        Conclusions = "";

        if (article is ResearchArticle researchArticle)
        {
            Methodology = researchArticle.Methodology;
            Results = researchArticle.Results;
        }
        else if (article is ReviewArticle reviewArticle)
        {
            Sources = reviewArticle.Sources;
            ReviewPeriod = reviewArticle.ReviewPeriod;
        }
        else if (article is CaseStudy caseStudy)
        {
            CaseDescription = caseStudy.CaseDescription;
            Conclusions = caseStudy.Conclusions;
        }
    }

    public bool ShouldSerializeMethodology() => !string.IsNullOrWhiteSpace(Methodology);
    public bool ShouldSerializeResults() => !string.IsNullOrWhiteSpace(Results);
    
    public bool ShouldSerializeSources() => Sources != null && Sources.Length > 0;
    public bool ShouldSerializeReviewPeriod() => !string.IsNullOrWhiteSpace(ReviewPeriod);
    
    public bool ShouldSerializeCaseDescription() => !string.IsNullOrWhiteSpace(CaseDescription);
    public bool ShouldSerializeConclusions() => !string.IsNullOrWhiteSpace(Conclusions);
}