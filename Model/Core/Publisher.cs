using Model.Interfaces;

namespace Model.Core;

public class Publisher
{
    private List<string> _themes;
    public string Name { get; private set; }
    public double Rating { get; private set; }
    public PublisherCitationStyle CitationStyle { get; private set; }
    public string[] Themes => _themes.ToArray();

    public Publisher(
        string name,
        double rating,
        List<string> themes,
        PublisherCitationStyle citationStyle = PublisherCitationStyle.Apa)
    {
        ValidateName(name);

        _themes = new List<string>();
        AddTheme(themes);

        Name = name;
        Rating = rating;
        CitationStyle = citationStyle;
    } 

    public void AddTheme (string theme)
    {
        if (!CheckAvailabilityTheme(theme)) 
            _themes.Add(theme);
    }
    public void AddTheme (List<string> themes)
    {
        foreach (var theme in themes) 
            AddTheme(theme);
    }
    public void RemoveTheme (string theme)
    {
        if (CheckAvailabilityTheme(theme))
            _themes.Remove(theme);
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