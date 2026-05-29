namespace Model.Core;

public abstract partial class Article
{
    public Publisher? Publisher { get; private set; }
    public void AddPublisher(Publisher publisher)
    {
        if (publisher == null) return;

        bool matchesTheme = KeyWords
            .Any(keyword => publisher.Themes
                .Any(theme => theme.Equals(keyword, StringComparison.OrdinalIgnoreCase)));

        if (!matchesTheme) return;

        Publisher = publisher;
        FormatCitation(publisher);
    }
    public void RemovePublisher() => Publisher = null;
}
