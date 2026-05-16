using Model.Core;
using Newtonsoft.Json.Linq;

namespace Model.Data;

public class JsonFileManager<T> : FileManager<T>
    where T : Article
{
    public JsonFileManager (string file_name, 
                            string file_extension) 
                            : base(file_name, file_extension, "json")
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;
        Directory.CreateDirectory(FolderPath!);

        JObject json_object = JObject.FromObject(obj);
        string string_json_object = json_object.ToString();

        File.WriteAllText(FullPath, string_json_object);
        
    }
    public T Deserialize()
    {
        if (!File.Exists(FullPath)) return null!;

        string string_json_object = File.ReadAllText(FullPath);
        if (string.IsNullOrWhiteSpace(string_json_object)) return null!;

        JObject json_object = JObject.Parse(string_json_object);
        object? obj = json_object.ToObject<T>();
        if (obj == null) return null!;

        return (T)obj;
    }
}
