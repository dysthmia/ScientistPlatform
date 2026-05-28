using System;
using System.IO;

namespace Model.Data;

public static class ExportHelper
{
    public static string GetDownloadsPath()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var localizedPath = Path.Combine(userProfile, "Загрузки");
        if (Directory.Exists(localizedPath)) return localizedPath;

        var path = Path.Combine(userProfile, "Downloads");
        if (Directory.Exists(path)) return path;

        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        return Directory.Exists(desktop) ? desktop : userProfile;
    }

    public static string GetSafeFileName(string title)
    {
        var safeName = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));
        if (safeName.Length > 100) safeName = safeName.Substring(0, 100);
        return safeName;
    }

    public static string EnsureExportFolder(string format)
    {
        var basePath = GetDownloadsPath();
        var folderPath = Path.Combine(basePath, format);
        Directory.CreateDirectory(folderPath);
        return folderPath;
    }
}
