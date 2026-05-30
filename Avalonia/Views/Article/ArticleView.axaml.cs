using System;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Model.Core;
using Model.Data;
using Model.Interfaces;

namespace ScientistPlatform.Views;

public partial class ArticleView : UserControl
{
    private Article _article = null!;
    private Action _goBack = null!;
    public ArticleView()
    {
        InitializeComponent();
    }

    public ArticleView(Article article, Action goBack)
    {
        InitializeComponent();
        
        _article = article;
        _goBack = goBack;

        ShowArticle(article);
    }

    private void ShowArticle(Article article)
    {
        TitleText.Text = article.Title;
        IssnText.Text = $"ISSN: {article.ISSN}";
        TypeText.Text = ArticleSearchService.GetArticleTypeName(article.Type);
        PublishedAtText.Text = $"Опубликовано: {article.PublishedAt.ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"))}";
        AuthorsText.Text = $"Авторы: {article.JoinAuthors()}";
        ArticleText.Text = article.Text;

        SetSection(MethodologyPanel, MethodologyText, null);
        SetSection(ResultsPanel, ResultsText, null);
        SetSection(CaseDescriptionPanel, CaseDescriptionText, null);
        SetSection(ConclusionsPanel, ConclusionsText, null);
        SetSection(ReviewPeriodPanel, ReviewPeriodText, null);
        SetSection(SourcesPanel, SourcesText, null);

        switch (article)
        {
            case ResearchArticle research:
                SetSection(MethodologyPanel, MethodologyText, research.Methodology);
                SetSection(ResultsPanel, ResultsText, research.Results);
                break;

            case CaseStudy caseStudy:
                SetSection(CaseDescriptionPanel, CaseDescriptionText, caseStudy.CaseDescription);
                SetSection(ConclusionsPanel, ConclusionsText, caseStudy.Conclusions);
                break;

            case ReviewArticle review:
                SetSection(ReviewPeriodPanel, ReviewPeriodText, review.ReviewPeriod);
                SetSection(SourcesPanel, SourcesText, string.Join(", ", review.Sources));
                break;
        }
    }

    private static void SetSection(StackPanel panel, SelectableTextBlock textBlock, string? text)
    {
        var hasText = !string.IsNullOrWhiteSpace(text);
        panel.IsVisible = hasText;
        textBlock.Text = hasText ? text : string.Empty;
    }
    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        _goBack();
    }
    private void DownloadJsonButton_Click(object? sender, RoutedEventArgs e)
    {
        SaveArticle((fileName, folderPath) => new JsonFileManager<Article>(fileName, folderPath));
    }
    private void DownloadXmlButton_Click(object? sender, RoutedEventArgs e)
    {
        SaveArticle((fileName, folderPath) => new XmlFileManager<Article>(fileName, folderPath));
    }
    private void DownloadTxtButton_Click(object? sender, RoutedEventArgs e)
    {
        SaveArticle((fileName, folderPath) => new TxtFileManager<Article>(fileName, folderPath));
    }
    private void SaveArticle(Func<string, string, FileManager<Article>> createManager)
    {
        var article = GetLatestArticle();
        var fileName = ExportHelper.GetSafeFileName(article.Title);
        var manager = createManager(fileName, ExportHelper.GetDownloadsPath());
        manager.Serialize(article);
    }
    private Article GetLatestArticle()
    {
        var repository = new ArticleRepository();
        return repository.Articles.FirstOrDefault(article => article.ISSN == _article.ISSN) ?? _article;
    }
}
