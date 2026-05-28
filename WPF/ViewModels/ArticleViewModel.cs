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
        
        BackCommand = new RelayCommand(_goBack);
    }

    public string Title { get; }
    public string Text { get; }
    public string Type { get; }
    public string PublishedAt { get; }
    public string Authors { get; }

    public IRelayCommand BackCommand { get; }
}
