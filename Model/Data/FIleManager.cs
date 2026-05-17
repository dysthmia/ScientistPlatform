using Model.Core;

namespace Model.Data;
public abstract class FileManager<T> where T : Article
{
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public string FolderPath { get; private set; }
    public string FullPath =>
        Path.Combine(FolderPath, $"{FileName}.{FileExtension}");

    public FileManager (string file_name,
                        string file_extension,
                        string folder_path)
    {
        FileName = file_name;
        FileExtension = file_extension;
        FolderPath = folder_path;
    }

    public void ChangeFileName (string new_name) => FileName = new_name;
    public void ChangeFolderPath (string new_folder_path) =>
        FolderPath = new_folder_path;

    public abstract void Serialize (T obj);
}
