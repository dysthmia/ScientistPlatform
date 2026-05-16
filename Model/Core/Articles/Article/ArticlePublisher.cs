namespace Model.Core;

public abstract partial class Article
{
    public Publisher Publisher { get; private set; }
    public void AddPublisher(Publisher publisher)
    {
        if (publisher == null) return;
        bool mathcesTheme = KeyWords
                            .Any(Keywords => publisher
                            .Themes
                            .Any(theme => theme
                            .Equals(Keywords, StringComparison.OrdinalIgnoreCase)));

        if (!mathcesTheme) return;
        Publisher = publisher;
    }
    public void RemovePublisher() => Publisher = null!;
}