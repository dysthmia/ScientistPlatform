using System;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Model.Core;

namespace WPF.ViewModels;

public class ArticleListItemViewModel
{
    private readonly Article _article;
    private readonly Action<Article> _onOpen;

    public string Title { get; }
    public string PublishedAt { get; }
    public string Authors { get; }
    public IRelayCommand OpenCommand { get; }

    public ArticleListItemViewModel(Article article, Action<Article> onOpen)
    {
        _article = article;
        _onOpen = onOpen;
        Title = article.Title;
        PublishedAt = article.PublishedAt.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        Authors = article.JoinAuthors();
        OpenCommand = new RelayCommand(() => _onOpen(_article));
    }
}