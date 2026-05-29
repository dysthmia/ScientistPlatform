namespace Model.Core;

public enum StorageFormat
{
    Json,
    Xml
}

public static class StorageConfig
{
    public static StorageFormat CurrentFormat { get; set; } = StorageFormat.Json;
}
