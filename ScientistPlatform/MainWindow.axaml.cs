using System;
using System.ComponentModel;
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

    public MainWindow()
    {
        InitializeComponent();
        
        CatalogPage.Initialize(OpenArticle);
        
        StartSearchCommand = new RelayCommand(OpenCatalog);
        
        DataContext = this;
    }

    public IRelayCommand StartSearchCommand { get; }
    
    public bool IsWelcomeVisible => !_isCatalogVisible && !_isArticleVisible;
    public bool IsCatalogVisible => _isCatalogVisible;
    public bool IsArticleVisible => _isArticleVisible;

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
