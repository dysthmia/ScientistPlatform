using Model.Core;

namespace Model.Data;
public abstract class FileManager<T> where T : Article
{
    public string FileName { get; private set; }
    public string FileExtension { get; private set; }
    public string FolderPath { get; private set; }
    public string FullPath =>
        Path.Combine(FolderPath, $"{FileName}.{FileExtension}");

    public FileManager (string fileName,
                        string fileExtension,
                        string folderPath)
    {
        FileName = fileName;
        FileExtension = fileExtension;
        FolderPath = folderPath;
    }

    public void ChangeFileName (string newName) => FileName = newName;
    public void ChangeFolderPath (string newFolderPath) =>
        FolderPath = newFolderPath;

    public abstract void Serialize (T obj);
}
