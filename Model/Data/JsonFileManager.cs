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

    public JsonFileManager (string file_name, 
                            string folder_path) 
                            : base(file_name, "json", folder_path)
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;
        Directory.CreateDirectory(FolderPath!);

        JObject json_object = JObject.FromObject(obj, JsonSerializer.Create(JsonSettings));

        File.WriteAllText(FullPath, json_object.ToString());
        
    }
    public T Deserialize()
    {
        if (!File.Exists(FullPath)) return null!;

        string string_json_object = File.ReadAllText(FullPath);
        if (string.IsNullOrWhiteSpace(string_json_object)) return null!;

        JObject json_object = JObject.Parse(string_json_object);
        object? obj = json_object.ToObject<T>(JsonSerializer.Create(JsonSettings));
        if (obj == null) return null!;

        return (T)obj;
    }
}
