using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Model.Core;

namespace ScientistPlatform;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private bool _isCatalogVisible;
    private bool _isArticleVisible;
    private object? _articlePage;
    private Publisher? _selectedPublisher;
    private string _submissionIssn = string.Empty;
    private string _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
    private bool _isSuccessMessage = false;

    public MainWindow()
    {
        InitializeComponent();
        
        Articles = new ArticleRepository().Articles
            .Select(article => new ArticleListItem(article, OpenArticle))
            .ToList();
        
        Publishers = PublisherRepository.GetAll().ToList();
        
        StartSearchCommand = new RelayCommand(OpenCatalog);
        SubmitArticleCommand = new RelayCommand(SubmitArticle, () => SelectedPublisher != null && !string.IsNullOrWhiteSpace(SubmissionIssn));
        
        DataContext = this;
    }

    public List<ArticleListItem> Articles { get; }
    public List<Publisher> Publishers { get; }
    public IRelayCommand StartSearchCommand { get; }
    public IRelayCommand SubmitArticleCommand { get; }
    
    public bool IsWelcomeVisible => !_isCatalogVisible && !_isArticleVisible;
    public bool IsCatalogVisible => _isCatalogVisible;
    public bool IsArticleVisible => _isArticleVisible;

    public Publisher? SelectedPublisher
    {
        get => _selectedPublisher;
        set
        {
            _selectedPublisher = value;
            _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
            _isSuccessMessage = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            SubmitArticleCommand.NotifyCanExecuteChanged();
        }
    }

    public string SubmissionIssn
    {
        get => _submissionIssn;
        set
        {
            _submissionIssn = value;
            _submissionMessage = "Ваша статья будет отправлена на проверку в выбранное издательство.";
            _isSuccessMessage = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            SubmitArticleCommand.NotifyCanExecuteChanged();
        }
    }

    public string SubmissionMessage => _submissionMessage;
    public string MessageForeground => _isSuccessMessage ? "#34A853" : (_submissionMessage.Contains("не соответствует") || _submissionMessage.Contains("не найдена") ? "#EA4335" : "#70757A");

    public object? ArticlePage 
    {
        get => _articlePage;
        set
        {
            _articlePage = value;
            OnPropertyChanged();
        }
    }

    private void OpenCatalog()
    {
        _isCatalogVisible = true;
        _isArticleVisible = false;
        OnPropertyChanged(nameof(IsWelcomeVisible));
        OnPropertyChanged(nameof(IsCatalogVisible));
        OnPropertyChanged(nameof(IsArticleVisible));
    }

    private void OpenArticle(Article article)
    {
        ArticlePage = new Views.ArticleView(article, OpenCatalog);
        _isCatalogVisible = false;
        _isArticleVisible = true;
        OnPropertyChanged(nameof(IsWelcomeVisible));
        OnPropertyChanged(nameof(IsCatalogVisible));
        OnPropertyChanged(nameof(IsArticleVisible));
    }

    private void SubmitArticle()
    {
        if (SelectedPublisher == null || string.IsNullOrWhiteSpace(SubmissionIssn)) return;

        var repository = new ArticleRepository();
        var article = repository.Articles.FirstOrDefault(a => a.ISSN == SubmissionIssn);

        if (article == null)
        {
            _submissionMessage = "Статья с таким ISSN не найдена.";
            _isSuccessMessage = false;
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            return;
        }

        bool matchesTheme = SelectedPublisher.Themes.Any(theme => 
            article.KeyWords.Contains(theme, StringComparer.OrdinalIgnoreCase));

        if (!matchesTheme)
        {
            _submissionMessage = "Тематика статьи не соответствует темам издательства.";
            _isSuccessMessage = false;
            OnPropertyChanged(nameof(SubmissionMessage));
            OnPropertyChanged(nameof(MessageForeground));
            return;
        }

        _submissionMessage = "Статья успешно отправлена!";
        _isSuccessMessage = true;
        OnPropertyChanged(nameof(SubmissionMessage));
        OnPropertyChanged(nameof(MessageForeground));

        _submissionIssn = string.Empty;
        _selectedPublisher = null;
        OnPropertyChanged(nameof(SubmissionIssn));
        OnPropertyChanged(nameof(SelectedPublisher));
        SubmitArticleCommand.NotifyCanExecuteChanged();
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ArticleListItem
{
    private readonly Article _article;
    private readonly Action<Article> _onOpen;

    public string Title { get; }
    public string PublishedAt { get; }
    public string Authors { get; }
    public IRelayCommand OpenCommand { get; }

    public ArticleListItem(Article article, Action<Article> onOpen)
    {
        _article = article;
        _onOpen = onOpen;
        Title = article.Title;
        PublishedAt = article.PublishedAt.ToString("dd MMMM yyyy", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
        Authors = article.JoinAuthors();
        OpenCommand = new RelayCommand(() => _onOpen(_article));
    }
}
