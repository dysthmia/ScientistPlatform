using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Model.Core;
using Model.Data;

namespace ScientistPlatform.Views;

public partial class CatalogView : UserControl
{
    private const string DefaultSubmissionMessage =
        "Ваша статья будет отправлена на проверку в выбранное издательство.";

    private readonly List<ArticleListItem> _articles = new();
    private Action<Article>? _openArticle;
    private bool _isUpdatingSubmissionForm;
    private bool _submissionSucceeded;
    public CatalogView()
    {
        InitializeComponent();
        SetSubmissionMessage(DefaultSubmissionMessage, "#70757A");
        UpdateSubmitButton();
    }

    public void Initialize(Action<Article> openArticle)
    {
        _openArticle = openArticle;

        var repository = new ArticleRepository();
        _articles.Clear();
        _articles.AddRange(repository.Articles.Select(article => new ArticleListItem(article)));

        ArticlesList.ItemsSource = null;
        ArticlesList.ItemsSource = _articles;

        PublishersComboBox.ItemsSource = PublisherRepository.GetAll();
        _submissionSucceeded = false;
        ClearSubmissionForm();
        ResetSubmissionMessage();
        UpdateSubmitButton();
    }

    private void ArticleButton_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: ArticleListItem item })
            _openArticle?.Invoke(item.Article);
    }
    private void PublisherComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ShouldResetSubmissionMessage())
        {
            _submissionSucceeded = false;
            ResetSubmissionMessage();
        }

        UpdateSubmitButton();
    }
    private void SubmissionIssnTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (ShouldResetSubmissionMessage())
        {
            _submissionSucceeded = false;
            ResetSubmissionMessage();
        }

        UpdateSubmitButton();
    }
    private void SubmitArticleButton_Click(object? sender, RoutedEventArgs e)
    {
        var publisher = PublishersComboBox.SelectedItem as Publisher;
        var issn = SubmissionIssnTextBox.Text?.Trim();

        if (publisher == null || string.IsNullOrWhiteSpace(issn))
            return;

        var repository = new ArticleRepository();
        var article = repository.Articles.FirstOrDefault(current =>
            string.Equals(current.ISSN, issn, StringComparison.OrdinalIgnoreCase));

        if (article == null)
        {
            SetSubmissionMessage("Статья с таким ISSN не найдена.", "#EA4335");
            return;
        }

        article.AddPublisher(publisher);

        if (!string.Equals(article.Publisher?.Name, publisher.Name, StringComparison.Ordinal))
        {
            SetSubmissionMessage("Тематика статьи не соответствует темам издательства.", "#EA4335");
            return;
        }

        repository.SaveArticle(article, article.ISSN.Replace(" ", "_"));

        _submissionSucceeded = true;
        ClearSubmissionForm();
        SetSubmissionMessage("Статья успешно отправлена!", "#34A853");
        UpdateSubmitButton();
    }

    private void DownloadAllJson_Click(object? sender, RoutedEventArgs e)
    {
        SaveAllArticles(
            "ArticlesJSON",
            (fileName, folderPath) => new JsonFileManager<Article>(fileName, folderPath));
    }
    private void DownloadAllXml_Click(object? sender, RoutedEventArgs e)
    {
        SaveAllArticles(
            "ArticlesXML",
            (fileName, folderPath) => new XmlFileManager<Article>(fileName, folderPath));
    }
    private void DownloadAllTxt_Click(object? sender, RoutedEventArgs e)
    {
        SaveAllArticles(
            "ArticlesTXT",
            (fileName, folderPath) => new TxtFileManager<Article>(fileName, folderPath));
    }
    private void SaveAllArticles(string folderName, Func<string, string, FileManager<Article>> createManager)
    {
        var repository = new ArticleRepository();
        var folderPath = ExportHelper.EnsureExportFolder(folderName);

        foreach (var item in _articles)
        {
            var article = repository.Articles.FirstOrDefault(current => current.ISSN == item.Article.ISSN)
                          ?? item.Article;
            var fileName = ExportHelper.GetSafeFileName(article.Title);
            var manager = createManager(fileName, folderPath);

            manager.Serialize(article);
        }
    }
    private void ResetSubmissionMessage()
    {
        SetSubmissionMessage(DefaultSubmissionMessage, "#70757A");
    }
    private void ClearSubmissionForm()
    {
        _isUpdatingSubmissionForm = true;

        try
        {
            SubmissionIssnTextBox.Text = string.Empty;
            PublishersComboBox.SelectedItem = null;
        }
        finally
        {
            _isUpdatingSubmissionForm = false;
        }
    }
    private bool ShouldResetSubmissionMessage()
    {
        if (_isUpdatingSubmissionForm)
            return false;

        return !_submissionSucceeded || !IsSubmissionFormEmpty();
    }
    private bool IsSubmissionFormEmpty() =>
        PublishersComboBox.SelectedItem == null &&
        string.IsNullOrWhiteSpace(SubmissionIssnTextBox.Text);
    private void SetSubmissionMessage(string message, string color)
    {
        SubmissionMessageText.Text = message;
        SubmissionMessageText.Foreground = Brush.Parse(color);
    }
    private void UpdateSubmitButton()
    {
        SubmitArticleButton.IsEnabled =
            PublishersComboBox.SelectedItem is Publisher &&
            !string.IsNullOrWhiteSpace(SubmissionIssnTextBox.Text);
    }
}
