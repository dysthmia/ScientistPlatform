using Model.Interfaces;

namespace Model.Core;

public static class PublisherRepository
{
    private static List<Publisher> Publishers =
    [
        new Publisher(
            "Science Press",
            4.8,
            new List<string> { "X", "Y", "study" },
            PublisherCitationStyle.Apa),

        new Publisher(
            "Academic Review",
            4.5,
            new List<string> { "Z", "review" },
            PublisherCitationStyle.Gost),

        new Publisher(
            "Case Reports Hub",
            4.2,
            new List<string> { "case", "event" },
            PublisherCitationStyle.Ieee)
    ];

    public static IReadOnlyList<Publisher> GetAll() => Publishers;
}
