using System.IO.Compression;
using LDAppUpdateTool_Publish.Services.InterFaces;
using LDAppUpdateTool_Publish.Share;

namespace LDAppUpdateTool_Publish.Services;

public class UpdateService : IUpdateService
{
    public void CreateAddOrUpdateZipFile(string zipPath)
    {
        var files = AppShareData.Instance!.UpdateData.AddOrUpdateFiles;

        var zipFilePath = Path.Combine(zipPath, AppConstant.ZipFileName);
        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }
        // Create a new zip file
        using var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
        foreach (var file in files)
        {
           var sourceFile = Path.Combine(AppShareData.BinPath, file);
            if (!File.Exists(sourceFile))
            {
                continue;
            }
            var entryFileName = Path.GetFileName(sourceFile);

            // Add each file to the zip
            archive.CreateEntryFromFile(sourceFile, entryFileName);
        }
    }

    public void CreateUpdateConfigFile(string path)
    {
        AppShareData.Instance!.UpdateData.SaveJsonFile(path);
    }

    public string GetZipFileHash(string zipPath)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        using var stream = File.OpenRead(zipPath);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
