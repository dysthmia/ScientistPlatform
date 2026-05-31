using Model.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Model.Data;

public class JsonFileManager<T> : FileManager<T>
    where T : Article
{
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Objects,
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
    };

    public JsonFileManager (string fileName, 
                            string folderPath) 
                            : base(fileName, "json", folderPath)
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;
        Directory.CreateDirectory(FolderPath!);

        JObject jsonObject = JObject.FromObject(obj, JsonSerializer.Create(JsonSettings));

        File.WriteAllText(FullPath, jsonObject.ToString());
        
    }
    public T Deserialize()
    {
        if (!File.Exists(FullPath)) return null!;

        string stringJsonObject = File.ReadAllText(FullPath);
        if (string.IsNullOrWhiteSpace(stringJsonObject)) return null!;

        JObject jsonObject = JObject.Parse(stringJsonObject);
        object? obj = jsonObject.ToObject<T>(JsonSerializer.Create(JsonSettings));
        if (obj == null) return null!;

        return (T)obj;
    }
}
