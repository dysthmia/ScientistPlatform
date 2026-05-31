namespace Model.Core;

public enum StorageFormat
{
    JSON,
    XML
}

public static class StorageConfig
{
    public static StorageFormat CurrentFormat { get; set; } = StorageFormat.JSON;
}
