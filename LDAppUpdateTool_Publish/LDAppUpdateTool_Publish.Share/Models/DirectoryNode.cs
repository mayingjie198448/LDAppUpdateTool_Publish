namespace LDAppUpdateTool_Publish.Share.Models;

public class DirectoryNodeModel
{
    public DirectoryNodeModel(string directoryName)
    {
        DirectoryName = directoryName;
        Files = new List<string>();
        SubDirectories = new List<DirectoryNodeModel>();
    }

    public string DirectoryName { get; set; }
    public List<string> Files { get; set; }
    public List<DirectoryNodeModel> SubDirectories { get; set; }

    public void LoadSubDirectoriesAndFiles()
    {
        if (Directory.Exists(this.DirectoryName))
        {
            var directories = Directory.GetDirectories(this.DirectoryName);
            if (directories.Any())
            {
                foreach (var directory in directories)
                {
                    var directoryNode = new DirectoryNodeModel(directory);
                    directoryNode.LoadSubDirectoriesAndFiles();
                    SubDirectories.Add(directoryNode);
                }
            }
            this.Files = Directory.GetFiles(this.DirectoryName).ToList();

        }
    }
}
