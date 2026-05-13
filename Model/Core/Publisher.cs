namespace Model.Core;

public class Publisher
{
    public string Name { get; private set; }
    public double Rating { get; private set; }
    public List<string> Themes { get; private set; }

    public Publisher(string name, double rating, List<string> themes)
    {
        if (themes == null) return;
        
        Name = name;
        Rating = rating;
        Themes = themes;
    } 

    private bool HasTheme (string theme) => Themes.Any(t => t == theme);
    public void AddTheme (string theme)
    {
        if (string.IsNullOrWhiteSpace(theme)) return;
        if (HasTheme(theme)) return;
        Themes.Add(theme);
    }
    public void RemoveTheme (string theme) => Themes.Remove(theme);
}