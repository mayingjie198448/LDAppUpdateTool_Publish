using System.Text.Json;

namespace LDAppUpdateTool_Publish.Share.Models;

public class UpdateDataModel
{
    public string AppVersion { get; set; }

    public string Hash { get; set; }

    public List<string> AddOrUpdateFiles { get; set; }

    public List<string> DeleteFiles { get; set; }

    public bool SaveJsonFile(string path)
    {
        try
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(Path.Combine(path,AppConstant.UpdateConfigFileName), json);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
