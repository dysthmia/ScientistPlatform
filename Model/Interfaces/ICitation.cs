namespace Model.Interfaces;

public interface ICitation
{
    string Citation { get; }
}

public enum PublisherCitationStyle
{
    Apa,
    Gost,
    Ieee
}