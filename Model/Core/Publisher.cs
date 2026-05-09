namespace Model.Core;

public class Publisher
{
    public string Name { get; private set; }
    public double Rating { get; private set; }
    public List<string> Themes { get; private set; }

    public Publisher(string name, double rating, List<string> themes)
    {
        Name = name;
        Rating = rating;
        Themes = themes;
    } 
}