using Model.Data;

namespace Model.Core;

public static class SubmissionService
{
    public static bool IsAlreadySubmitted(Article article)
    {
        if (article == null) return false;
        
        var repository = new ArticleRepository();
        
        var existingArticle = repository.Articles
            .FirstOrDefault(a => a == article);

        return existingArticle?.Publisher != null;
    }
}
