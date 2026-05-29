using Model.Interfaces;

namespace Model.Core;

public static class PublisherRepository
{
    private static List<Publisher> Publishers =
    [
        new Publisher(
            "Science Press",
            4.8,
            new List<string> 
            { 
                "study", "urban ecology", "microclimate", "green infrastructure", 
                "climate adaptation", "remote sensing", "drought", "soil moisture", 
                "precision agriculture", "open science", "reproducibility", 
                "materials science", "laboratory notebook", "biodiversity", 
                "acoustics", "forest restoration", "machine learning" 
            },
            PublisherCitationStyle.Apa),

        new Publisher(
            "Academic Review",
            4.5,
            new List<string> 
            { 
                "review", "digital twin", "smart city", "urban planning", 
                "sustainability", "microplastics", "freshwater", "ecotoxicology", 
                "water quality", "medical imaging", "artificial intelligence", 
                "diagnostics", "clinical safety" 
            },
            PublisherCitationStyle.Gost),

        new Publisher(
            "Case Reports Hub",
            4.2,
            new List<string> 
            { 
                "case", "water management", "cyanobacteria", "public health", 
                "restoration", "energy efficiency", "campus", "building retrofit", 
                "air quality", "citizen science", "sensors", "urban health" 
            },
            PublisherCitationStyle.Ieee)
    ];

    public static IReadOnlyList<Publisher> GetAll() => Publishers;
}
