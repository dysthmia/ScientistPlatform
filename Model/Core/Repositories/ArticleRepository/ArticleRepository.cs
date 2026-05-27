using Model.Interfaces;

namespace Model.Core;

public partial class ArticleRepository
{
    private Article[] _articles = Array.Empty<Article>();
    public Article[] Articles => _articles.ToArray();
    public ArticleRepository()
    {
        LoadAll();
    }

    private void ReplaceArticles(IEnumerable<Article> articles) =>
        _articles = articles.ToArray();
    private void AddOrReplaceArticle(Article article)
    {
        ArgumentNullException.ThrowIfNull(article);

        var existingIndex = Array.FindIndex(_articles, current => current.ISSN == article.ISSN);

        if (existingIndex < 0)
        {
            _articles = _articles.Append(article).ToArray();
            return;
        }

        _articles[existingIndex] = article;
    }
}
