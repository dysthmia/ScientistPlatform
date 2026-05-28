using System;
using System.Globalization;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using Model.Core;
using Model.Data;
using Model.Interfaces;

namespace ScientistPlatform.Views;

public partial class ArticleView : UserControl
{
    private readonly Article _article;
    private readonly Action _goBack;

    public ArticleView()
    {
        InitializeComponent();
    }

    public ArticleView(Article article, Action goBack)
    {
        InitializeComponent();
        
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
        ISSN = _article.ISSN;

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
        DownloadJsonCommand = new RelayCommand(DownloadJson);
        DownloadXmlCommand = new RelayCommand(DownloadXml);

        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public string Title { get; }
    public string Text { get; }
    public string Type { get; }
    public string PublishedAt { get; }
    public string Authors { get; }
    public string ISSN { get; }

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
    public IRelayCommand DownloadJsonCommand { get; }
    public IRelayCommand DownloadXmlCommand { get; }

    private void DownloadJson()
    {
        var downloadsPath = GetDownloadsPath();
        var fileName = GetSafeFileName();
        var manager = new JsonFileManager<Article>(fileName, downloadsPath);
        manager.Serialize(_article);
    }

    private void DownloadXml()
    {
        var downloadsPath = GetDownloadsPath();
        var fileName = GetSafeFileName();
        var manager = new XmlFileManager<Article>(fileName, downloadsPath);
        manager.Serialize(_article);
    }

    private string GetDownloadsPath()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var path = Path.Combine(userProfile, "Downloads");
        if (Directory.Exists(path)) return path;

        var localizedPath = Path.Combine(userProfile, "Загрузки");
        if (Directory.Exists(localizedPath)) return localizedPath;

        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return Directory.Exists(desktop) ? desktop : userProfile;
    }

    private string GetSafeFileName()
    {
        var safeName = string.Join("_", Title.Split(Path.GetInvalidFileNameChars()));
        if (safeName.Length > 100) safeName = safeName.Substring(0, 100);
        return safeName;
    }
}
