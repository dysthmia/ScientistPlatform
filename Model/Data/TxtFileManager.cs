using System.Text;
using Model.Core;

namespace Model.Data;

public class TxtFileManager<T> : FileManager<T>
    where T : Article
{
    public TxtFileManager(string fileName, string folderPath)
        : base(fileName, "txt", folderPath)
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;

        Directory.CreateDirectory(FolderPath);

        var content = new StringBuilder();
        content.AppendLine($"Название: {obj.Title}");
        content.AppendLine($"Тип: {GetTypeName(obj)}");
        content.AppendLine($"Дата публикации: {obj.PublishedAt:dd.MM.yyyy}");
        content.AppendLine($"Авторы: {string.Join(", ", obj.Authors.Select(author => author.Name))}");
        content.AppendLine($"Ключевые слова: {string.Join(", ", obj.KeyWords)}");

        if (obj.Publisher != null)
            content.AppendLine($"Издательство: {obj.Publisher.Name}");

        content.AppendLine();
        content.AppendLine("Текст статьи:");
        content.AppendLine(obj.Text);

        AppendArticleDetails(content, obj);

        File.WriteAllText(FullPath, content.ToString());
    }

    private static string GetTypeName(Article article) =>
        article switch
        {
            ResearchArticle => nameof(ResearchArticle),
            ReviewArticle => nameof(ReviewArticle),
            CaseStudy => nameof(CaseStudy),
            _ => article.Type.ToString()
        };

    private static void AppendArticleDetails(StringBuilder content, Article article)
    {
        switch (article)
        {
            case ResearchArticle researchArticle:
                content.AppendLine();
                content.AppendLine("Методология:");
                content.AppendLine(researchArticle.Methodology);
                content.AppendLine();
                content.AppendLine("Результаты:");
                content.AppendLine(researchArticle.Results);
                break;

            case ReviewArticle reviewArticle:
                content.AppendLine();
                content.AppendLine($"Период обзора: {reviewArticle.ReviewPeriod}");
                content.AppendLine("Источники:");
                foreach (var source in reviewArticle.Sources)
                    content.AppendLine($"- {source}");
                break;

            case CaseStudy caseStudy:
                content.AppendLine();
                content.AppendLine("Описание случая:");
                content.AppendLine(caseStudy.CaseDescription);
                content.AppendLine();
                content.AppendLine("Выводы:");
                content.AppendLine(caseStudy.Conclusions);
                break;
        }
    }
}
