namespace Model.Core;

class Author
{
    public string Name { get; private set; }
    public string ORCID { get; private set; }
    public List<Article> Articles { get; private set; }

    public Author(string name)
    {
        Name = name;
        ORCID = GenerateORCID();
        Articles = new List<Article>();
    }

    private static string GenerateORCID()
    {
        var random = new Random();
        return $"{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}-{random.Next(0000, 9999):D4}";
    }
}