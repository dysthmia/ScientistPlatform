using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Model.Core;

namespace WPF.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isCatalogVisible;
    private bool _isArticleVisible;

    public MainWindowViewModel()
    {
        Articles = new ArticleRepository().Articles
            .Select(article => new ArticleListItemViewModel(article, OpenArticle))
            .ToList();
        StartSearchCommand = new RelayCommand(OpenCatalog);
    }

    public IReadOnlyList<ArticleListItemViewModel> Articles { get; }
    public IRelayCommand StartSearchCommand { get; }
    
    public bool IsWelcomeVisible => !_isCatalogVisible && !_isArticleVisible;
    public bool IsCatalogVisible => _isCatalogVisible;
    public bool IsArticleVisible => _isArticleVisible;

    private ArticleViewModel? _articlePage;
    public ArticleViewModel? ArticlePage 
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
        ArticlePage = new ArticleViewModel(article, OpenCatalog);
        _isCatalogVisible = false;
        _isArticleVisible = true;
        OnPropertyChanged(nameof(IsWelcomeVisible));
        OnPropertyChanged(nameof(IsCatalogVisible));
        OnPropertyChanged(nameof(IsArticleVisible));
    }
}
