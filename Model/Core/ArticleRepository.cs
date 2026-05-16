using Newtonsoft.Json;

namespace Model.Core;
public static class ArticleRepository
{
    private const string _folder = "json";
    private const string _extension = "json";

    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Objects,
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
    };

    public static List<Article> LoadAll()
    {
        Directory.CreateDirectory(_folder);

        var files = Directory.EnumerateFiles(_folder, $"*.{_extension}").ToList();
        if (!files.Any())
        {
            SeedSampleFiles();
            files = Directory.EnumerateFiles(_folder, $"*.{_extension}").ToList();
        }

        var result = new List<Article>();

        foreach (var file in files)
        {
            try
            {
                var text = File.ReadAllText(file);
                if (string.IsNullOrWhiteSpace(text)) continue;

                var article = JsonConvert.DeserializeObject<Article>(text, JsonSettings);
                if (article != null) result.Add(article);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to deserialize '{file}': {ex.GetType().Name}: {ex.Message}");
            }
        }

        return result;
    }

    private static void SeedSampleFiles()
    {
        var samples = new List<Article>
        {
            new ResearchArticle(
                "Effects of X on Y",
                "Introductory study of X...",
                new[] { "X", "Y", "study" },
                new List<Author> { new Author("Ivan Petrov") },
                methodology: "Quantitative",
                results: "Significant"),

            new ReviewArticle(
                "A review of Z literature",
                "Comprehensive review...",
                new[] { "Z", "review" },
                new List<Author> { new Author("Anna Ivanova"), new Author("Pavel Sidorov") },
                sources: new[] { "Source A", "Source B" },
                reviewPeriod: "2010-2020"),

            new CaseStudy(
                "Case study: unusual event",
                "Detailed account of the case...",
                new[] { "case", "event" },
                new List<Author> { new Author("Olga Smirnova") },
                caseDescription: "Description here",
                conclusions: "Conclusions here")
        };

        Directory.CreateDirectory(_folder);

        int i = 1;
        foreach (var article in samples)
        {
            var file = Path.Combine(_folder, $"article_{i}.{_extension}");
            var text = JsonConvert.SerializeObject(article, JsonSettings);
            File.WriteAllText(file, text);
            i++;
        }
    }
}