using System;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using Model.Core;
using Model.Interfaces;

namespace WPF.ViewModels;

public class ArticleViewModel : ViewModelBase
{
    private readonly Article _article;
    private readonly Action _goBack;

    public ArticleViewModel(Article article, Action goBack)
    {
        _article = article;
        _goBack = goBack;
        
        Title = _article.Title;
        Text = _article.Text;
        Type = _article.Type switch
        {
            ArticleType.Research => "Исследование",
            ArticleType.Review => "Обзор",
            ArticleType.CaseStudy => "Кейс-стади",
            _ => "Статья"
        };
        PublishedAt = _article.PublishedAt.ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        Authors = _article.JoinAuthors();

        if (_article is ResearchArticle research)
        {
            Methodology = research.Methodology;
            Results = research.Results;
        }
        else if (_article is CaseStudy caseStudy)
        {
            CaseDescription = caseStudy.CaseDescription;
            Conclusions = caseStudy.Conclusions;
        }
        else if (_article is ReviewArticle review)
        {
            ReviewPeriod = review.ReviewPeriod;
            Sources = string.Join(", ", review.Sources);
        }
        
        BackCommand = new RelayCommand(_goBack);
    }

    public string Title { get; }
    public string Text { get; }
    public string Type { get; }
    public string PublishedAt { get; }
    public string Authors { get; }

    public string? Methodology { get; }
    public string? Results { get; }
    public string? CaseDescription { get; }
    public string? Conclusions { get; }
    public string? Sources { get; }
    public string? ReviewPeriod { get; }

    public bool HasMethodology => !string.IsNullOrEmpty(Methodology);
    public bool HasResults => !string.IsNullOrEmpty(Results);
    public bool HasCaseDescription => !string.IsNullOrEmpty(CaseDescription);
    public bool HasConclusions => !string.IsNullOrEmpty(Conclusions);
    public bool HasSources => !string.IsNullOrEmpty(Sources);
    public bool HasReviewPeriod => !string.IsNullOrEmpty(ReviewPeriod);

    public IRelayCommand BackCommand { get; }
}
