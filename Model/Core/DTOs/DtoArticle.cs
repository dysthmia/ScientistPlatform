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

    // Publisher
    public string PublisherName { get; set; } = "";
    public double PublisherRating { get; set; }
    public PublisherCitationStyle PublisherCitationStyle { get; set; }
    public string[] PublisherThemes { get; set; } = Array.Empty<string>();

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

        PublisherName = "";
        PublisherRating = 0;
        PublisherCitationStyle = PublisherCitationStyle.Apa;
        PublisherThemes = Array.Empty<string>();

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
        if (article is CaseStudy caseStudy)
        {
            CaseDescription = caseStudy.CaseDescription;
            Conclusions = caseStudy.Conclusions;
        }

        if (article.Publisher != null)
        {
            PublisherName = article.Publisher.Name;
            PublisherRating = article.Publisher.Rating;
            PublisherCitationStyle = article.Publisher.CitationStyle;
            PublisherThemes = article.Publisher.Themes;
        }
    }

    public Article ToArticle()
    {
        Article article = Type switch
        {   
            ArticleType.Research => new ResearchArticle(Title, Text, Keywords, AuthorsNames.Select(n => new Author(n)).ToList(), PublishedAt, Methodology, Results, ISSN),
            ArticleType.Review => new ReviewArticle(Title, Text, Keywords, PublishedAt, AuthorsNames.Select(n => new Author(n)).ToList(), Sources, ReviewPeriod, ISSN),
            ArticleType.CaseStudy => new CaseStudy(Title, Text, Keywords, AuthorsNames.Select(n => new Author(n)).ToList(), PublishedAt, CaseDescription, Conclusions, ISSN),
            _ => throw new ArgumentException("Invalid article type")
        };

        if (!string.IsNullOrWhiteSpace(PublisherName))
        {
            var publisher = new Publisher(
                PublisherName,
                PublisherRating,
                PublisherThemes?.ToList() ?? new List<string>(),
                PublisherCitationStyle);

            article.AddPublisher(publisher);
        }

        return article;
    }

    public bool ShouldSerializeMethodology() => !string.IsNullOrWhiteSpace(Methodology);
    public bool ShouldSerializeResults() => !string.IsNullOrWhiteSpace(Results);
    
    public bool ShouldSerializeSources() => Sources != null && Sources.Length > 0;
    public bool ShouldSerializeReviewPeriod() => !string.IsNullOrWhiteSpace(ReviewPeriod);
    
    public bool ShouldSerializeCaseDescription() => !string.IsNullOrWhiteSpace(CaseDescription);
    public bool ShouldSerializeConclusions() => !string.IsNullOrWhiteSpace(Conclusions);

    public bool ShouldSerializePublisherName() => !string.IsNullOrWhiteSpace(PublisherName);
    public bool ShouldSerializePublisherRating() => !string.IsNullOrWhiteSpace(PublisherName);
    public bool ShouldSerializePublisherCitationStyle() => !string.IsNullOrWhiteSpace(PublisherName);
    public bool ShouldSerializePublisherThemes() => PublisherThemes != null && PublisherThemes.Length > 0;
}
