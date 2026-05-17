using Model.Data;

namespace Model.Core;
public static class ArticleRepository
{
    private const string Folder = "json";
    private const string Extension = "json";

    public static List<Article> LoadAll()
    {
        Directory.CreateDirectory(Folder);

        if (!Directory.EnumerateFiles(Folder, $"*.{Extension}").Any())
            SeedSampleFiles();

        var files = Directory.EnumerateFiles(Folder, $"*.{Extension}").ToList();

        var result = new List<Article>();

        foreach (var file in files)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var manager = new JsonFileManager<Article>(fileName, Extension);
                manager.ChangeFolderPath(Folder);

                var article = manager.Deserialize();
                if (article != null)
                {
                    article.FormatDefaultCitation();
                    result.Add(article);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load article from file '{file}': {ex.Message}", ex);
            }
        }

        return result;
    }

    public static void SaveArticle(Article article, string fileName)
    {
        try
        {
            var manager = new JsonFileManager<Article>(fileName, Extension);
            manager.ChangeFolderPath(Folder);
            manager.Serialize(article);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save article: {ex.Message}", ex);
        }
    }

    private static void SeedSampleFiles()
    {
        var samples = new List<(Article article, string fileName)>
        {
            (
                new ResearchArticle(
                    "Effects of X on Y",
                    "Introductory study of X...",
                    new[] { "X", "Y", "study" },
                    new List<Author> { new Author("Ivan Petrov") },
                    methodology: "Quantitative",
                    results: "Significant"),
                "article_1"
            ),
            (
                new ReviewArticle(
                    "A review of Z literature",
                    "Comprehensive review...",
                    new[] { "Z", "review" },
                    new List<Author> { new Author("Anna Ivanova"), new Author("Pavel Sidorov") },
                    sources: new[] { "Source A", "Source B" },
                    reviewPeriod: "2010-2020"),
                "article_2"
            ),
            (
                new CaseStudy(
                    "Case study: unusual event",
                    "Detailed account of the case...",
                    new[] { "case", "event" },
                    new List<Author> { new Author("Olga Smirnova") },
                    caseDescription: "Description here",
                    conclusions: "Conclusions here"),
                "article_3"
            )
        };

        foreach (var (article, fileName) in samples)
        {
            try
            {
                SaveArticle(article, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to seed article '{fileName}': {ex.Message}", ex);
            }
        }
    }
}
