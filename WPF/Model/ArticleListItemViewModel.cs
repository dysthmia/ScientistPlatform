using System.Globalization;
using System.Linq;
using Model.Core;
namespace WPF.ViewModels;
public class ArticleListItemViewModel
{
    public string Title { get; }
    public string PublishedAt { get; }
    public string Authors { get; }
    public ArticleListItemViewModel(Article article)
    {
        Title = article.Title;
        PublishedAt = article.PublishedAt.ToString("dd MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU"));
        Authors = string.Join(", ", article.Authors.Select(author => author.Name));
    }
}