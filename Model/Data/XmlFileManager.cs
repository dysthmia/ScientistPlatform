using System.Xml.Serialization;
using Model.Core;
using Model.Core.DTOs;
using Model.Interfaces;

namespace Model.Data;

public class XmlFileManager<T> : FileManager<T>
    where T : Article
{
    public XmlFileManager  (string fileName, 
                            string folderPath) 
                            : base(fileName, "xml", folderPath)
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;
        Directory.CreateDirectory(FolderPath);

        var ser = new XmlSerializer(typeof(DtoArticle));
        var dtoArticle = new DtoArticle(obj);

        using (var sw = new StreamWriter(FullPath))
        {
            ser.Serialize(sw, dtoArticle);
        }
    }

    public T Deserialize()
    {
        if (!File.Exists(FullPath)) return null!;

        var ser = new XmlSerializer(typeof(DtoArticle));
        using (var sr = new StreamReader(FullPath))
        {
            var dto = (DtoArticle)ser.Deserialize(sr)!;
            return (T)dto.ToArticle();
        }
    }
}
