using System.Xml.Serialization;
using Model.Core;
using Model.Core.DTOs;
using Model.Interfaces;

namespace Model.Data;

public class XmlFileManager<T> : FileManager<T>
    where T : Article
{
    public XmlFileManager  (string file_name, 
                            string folder_path) 
                            : base(file_name, "xml", folder_path)
    {
    }

    public override void Serialize(T obj)
    {
        if (obj == null) return;
        Directory.CreateDirectory(FolderPath);

        var ser = new XmlSerializer(typeof(DtoArticle));
        var dto_article = new DtoArticle(obj);

        using (var sw = new StreamWriter(FullPath))
        {
            ser.Serialize(sw,dto_article);
        }
    }
}
