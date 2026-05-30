using System;
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Core;

public enum SortMode
{
    AlphabeticalAsc,
    AlphabeticalDesc,
    DateDesc,
    DateAsc
}

public class SearchCriteria
{
    public string? Criterion { get; set; }
    public string? Value { get; set; }
}

public static class ArticleSearchService
{
    public static IEnumerable<Article> FilterAndSort(
        IEnumerable<Article> articles,
        string? searchText,
        SortMode sortMode,
        SearchCriteria? criteria)
    {
        var filtered = articles;

        if (criteria != null && !string.IsNullOrEmpty(criteria.Criterion) && !string.IsNullOrEmpty(criteria.Value))
        {
            var val = criteria.Value;
            filtered = filtered.Where(article =>
                criteria.Criterion switch
                {
                    "Title" => article.Title.Contains(val, StringComparison.OrdinalIgnoreCase),
                    "ISSN" => article.ISSN.Contains(val, StringComparison.OrdinalIgnoreCase),
                    "KeyWords" => article.KeyWords.Any(k => k.Contains(val, StringComparison.OrdinalIgnoreCase)),
                    "Type" => GetArticleTypeName(article.Type).Contains(val, StringComparison.OrdinalIgnoreCase),
                    "PublishedAt" => article.PublishedAt.ToString("dd.MM.yyyy").Contains(val),
                    _ => true
                });
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            var searchWords = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            filtered = filtered.Where(article =>
            {
                bool titleMatches = searchWords.All(word => 
                    article.Title.Contains(word, StringComparison.OrdinalIgnoreCase));
                
                bool authorMatches = article.Authors.Any(a => 
                    searchWords.All(word => a.Name.Contains(word, StringComparison.OrdinalIgnoreCase)));

                return titleMatches || authorMatches;
            });
        }

        return sortMode switch
        {
            SortMode.AlphabeticalAsc => filtered.OrderBy(a => a.Title),
            SortMode.AlphabeticalDesc => filtered.OrderByDescending(a => a.Title),
            SortMode.DateDesc => filtered.OrderByDescending(a => a.PublishedAt),
            SortMode.DateAsc => filtered.OrderBy(a => a.PublishedAt),
            _ => filtered
        };
    }

    public static string GetArticleTypeName(ArticleType type) =>
        type switch
        {
            ArticleType.Research => "Исследование",
            ArticleType.Review => "Обзор",
            ArticleType.CaseStudy => "Кейс-стади",
            _ => "Статья"
        };
}
