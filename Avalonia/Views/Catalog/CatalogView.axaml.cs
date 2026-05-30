using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Model.Core;
using Model.Data;
using Model.Interfaces;

namespace ScientistPlatform.Views;

public partial class CatalogView : UserControl
{
    private const string DefaultSubmissionMessage =
        "Ваша статья будет отправлена на проверку в выбранное издательство.";

    private readonly List<ArticleListItem> _articles = new();
    private Action<Article>? _openArticle;
    private Action? _openCitation;
    private bool _isUpdatingSubmissionForm;
    private bool _submissionSucceeded;

    public CatalogView()
    {
        InitializeComponent();
        SetSubmissionMessage(DefaultSubmissionMessage, "#70757A");
        UpdateSubmitButton();
    }

    private enum SortMode
    {
        AlphabeticalAsc,
        AlphabeticalDesc,
        DateDesc,
        DateAsc
    }

    private string? _activeSearchText;
    private SortMode _activeSortMode = SortMode.AlphabeticalAsc;
    private string? _activeCriterion;
    private string? _activeCriteriaValue;

    public void Initialize(Action<Article> openArticle, Action openCitation)
    {
        _openArticle = openArticle;
        _openCitation = openCitation;

        var repository = new ArticleRepository();
        _articles.Clear();
        _articles.AddRange(repository.Articles.Select(article => new ArticleListItem(article)));

        PublishersComboBox.ItemsSource = PublisherRepository.GetAll();
        Logo_Click(null, new RoutedEventArgs());
    }

    private void MainSearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchButton.IsEnabled = !string.IsNullOrWhiteSpace(MainSearchTextBox.Text);
    }

    private void MainSearchTextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if ((e.Key == Avalonia.Input.Key.Enter || e.Key == Avalonia.Input.Key.Return) && SearchButton.IsEnabled)
        {
            SearchButton_Click(null, new RoutedEventArgs());
        }
    }

    private void SearchButton_Click(object? sender, RoutedEventArgs e)
    {
        _activeSearchText = MainSearchTextBox.Text?.Trim();
        UpdateArticlesList();
    }

    private void CriteriaInputs_Changed(object? sender, RoutedEventArgs e)
    {
        ApplyCriteriaButton.IsEnabled = CriteriaComboBox.SelectedItem != null && 
                                        !string.IsNullOrWhiteSpace(CriteriaValueTextBox.Text);
    }

    private void ApplyCriteria_Click(object? sender, RoutedEventArgs e)
    {
        var selectedItem = CriteriaComboBox.SelectedItem as ComboBoxItem;
        _activeCriterion = selectedItem?.Tag?.ToString();
        _activeCriteriaValue = CriteriaValueTextBox.Text?.Trim();
        
        CriteriaButton.Flyout?.Hide();
        UpdateArticlesList();
    }

    private void ApplySort_Click(object? sender, RoutedEventArgs e)
    {
        if (SortAZRadio.IsChecked == true) _activeSortMode = SortMode.AlphabeticalAsc;
        else if (SortZARadio.IsChecked == true) _activeSortMode = SortMode.AlphabeticalDesc;
        else if (SortNewestRadio.IsChecked == true) _activeSortMode = SortMode.DateDesc;
        else if (SortOldestRadio.IsChecked == true) _activeSortMode = SortMode.DateAsc;
        
        FilterButton.Flyout?.Hide();
        UpdateArticlesList();
    }

    private void Logo_Click(object? sender, RoutedEventArgs e)
    {
        _activeSearchText = null;
        _activeSortMode = SortMode.AlphabeticalAsc;
        _activeCriterion = null;
        _activeCriteriaValue = null;

        MainSearchTextBox.Text = string.Empty;
        CriteriaValueTextBox.Text = string.Empty;
        CriteriaComboBox.SelectedItem = null;
        SortAZRadio.IsChecked = true;

        _submissionSucceeded = false;
        ClearSubmissionForm();
        ResetSubmissionMessage();
        UpdateSubmitButton();

        UpdateArticlesList();
    }


    private void UpdateArticlesList()
    {
        IEnumerable<ArticleListItem> filtered = _articles;

        // Apply Criteria Filter
        if (!string.IsNullOrEmpty(_activeCriterion) && !string.IsNullOrEmpty(_activeCriteriaValue))
        {
            var criteriaWords = _activeCriteriaValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            filtered = filtered.Where(item =>
            {
                var article = item.Article;
                return _activeCriterion switch
                {
                    "Title" => criteriaWords.All(word => article.Title.Contains(word, StringComparison.OrdinalIgnoreCase)),
                    "ISSN" => article.ISSN.Contains(_activeCriteriaValue, StringComparison.OrdinalIgnoreCase),
                    "KeyWords" => article.KeyWords.Any(k => k.Contains(_activeCriteriaValue, StringComparison.OrdinalIgnoreCase)),
                    "Type" => GetArticleTypeName(article.Type).Contains(_activeCriteriaValue, StringComparison.OrdinalIgnoreCase),
                    "PublishedAt" => article.PublishedAt.ToString("dd.MM.yyyy").Contains(_activeCriteriaValue),
                    _ => true
                };
            });
        }

        // Apply Main Search (Title and Author.Name)
        if (!string.IsNullOrEmpty(_activeSearchText))
        {
            var searchWords = _activeSearchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            filtered = filtered.Where(item =>
            {
                var article = item.Article;
                
                bool titleMatches = searchWords.All(word => 
                    article.Title.Contains(word, StringComparison.OrdinalIgnoreCase));
                
                bool authorMatches = article.Authors.Any(a => 
                    searchWords.All(word => a.Name.Contains(word, StringComparison.OrdinalIgnoreCase)));

                return titleMatches || authorMatches;
            });
        }

        // Apply Sort
        filtered = _activeSortMode switch
        {
            SortMode.AlphabeticalAsc => filtered.OrderBy(item => item.Article.Title),
            SortMode.AlphabeticalDesc => filtered.OrderByDescending(item => item.Article.Title),
            SortMode.DateDesc => filtered.OrderByDescending(item => item.Article.PublishedAt),
            SortMode.DateAsc => filtered.OrderBy(item => item.Article.PublishedAt),
            _ => filtered
        };

        ArticlesList.ItemsSource = null;
        ArticlesList.ItemsSource = filtered.ToList();
    }

    private static string GetArticleTypeName(ArticleType type) =>
        type switch
        {
            ArticleType.Research => "Исследование",
            ArticleType.Review => "Обзор",
            ArticleType.CaseStudy => "Кейс-стади",
            _ => "Статья"
        };

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

        if (SubmissionService.IsAlreadySubmitted(article))
        {
            SetSubmissionMessage("Эта статья уже принадлежит издательству.", "#FBBC05");
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

    private void OpenCitationView_Click(object? sender, RoutedEventArgs e)
    {
        _openCitation?.Invoke();
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
