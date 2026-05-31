using System;
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

namespace Model.Core;

public delegate bool ArticleFilterDelegate(Article article);

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
            ArticleFilterDelegate criteriaFilter = CreateCriteriaFilter(criteria.Criterion, criteria.Value);
            filtered = filtered.Where(article => criteriaFilter(article));
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            ArticleFilterDelegate textFilter = CreateSearchTextFilter(searchText);
            filtered = filtered.Where(article => textFilter(article));
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

    private static ArticleFilterDelegate CreateCriteriaFilter(string criterion, string value)
    {
        string val = value.Trim();
        return criterion switch
        {
            "Title" => a => a.Title.Contains(val, StringComparison.OrdinalIgnoreCase),
            "ISSN" => a => a.ISSN.Contains(val, StringComparison.OrdinalIgnoreCase),
            "KeyWords" => a => a.KeyWords.Any(k => k.Contains(val, StringComparison.OrdinalIgnoreCase)),
            "Type" => a => GetArticleTypeName(a.Type).Contains(val, StringComparison.OrdinalIgnoreCase),
            "PublishedAt" => a => a.PublishedAt.ToString("dd.MM.yyyy").Contains(val),
            _ => a => true
        };
    }

    private static ArticleFilterDelegate CreateSearchTextFilter(string searchText)
    {
        var searchWords = searchText.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return article =>
        {
            bool titleMatches = searchWords.All(word => 
                article.Title.Contains(word, StringComparison.OrdinalIgnoreCase));
            
            bool authorMatches = article.Authors.Any(a => 
                searchWords.All(word => a.Name.Contains(word, StringComparison.OrdinalIgnoreCase)));

            return titleMatches || authorMatches;
        };
    }

    public static string GetArticleTypeName(ArticleType type) =>
        type switch
        {
            ArticleType.Research => "Исследование",
            ArticleType.Review => "Обзор",
            ArticleType.CaseStudy => "Тематическое исследование",
            _ => "Статья"
        };
}
