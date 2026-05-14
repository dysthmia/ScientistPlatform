namespace Model.Core;

public class Publisher
{
    private List<string> _themes;
    public string Name { get; private set; }
    public double Rating { get; private set; }
    public string[] Themes => _themes.ToArray();

    public Publisher(string name, double rating, List<string> themes)
    {      
        ValidateName(name);
        _themes = CheckAndCopyThemes(themes);

        Name = name;
        Rating = rating;
    } 

    public void AddTheme (string theme)
    {
        if (!CheckAvailabilityTheme(theme)) _themes.Add(theme);
    }
    public void RemoveTheme (string theme)
    {
        if (CheckAvailabilityTheme(theme)) _themes.Remove(theme);
    }

    private List<string> CheckAndCopyThemes(List<string> themes)
    {
        List<string> checked_themes = new List<string>();
        foreach(var theme in themes) AddTheme(theme);
        return checked_themes;
    }
    private bool CheckAvailabilityTheme (string theme)
    {
        if (string.IsNullOrWhiteSpace(theme))
            throw new ArgumentException(nameof(theme), "Тема не должна быть пустой");
        
        return _themes.Any(t => t == theme);
    }
    private void ValidateName (string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name), "Имя Publisher не должно быть пустым");
    }
}