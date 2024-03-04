using LDAppUpdateTool_Publish.Share.Models;

namespace LDAppUpdateTool_Publish.Share;

public class AppShareData
{
    private static AppShareData _instance;

    private static readonly object Lock = new();

    public static AppShareData Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppShareData();
                    }
                }
            }

            return _instance;
        }
    }

    public UpdateDataModel UpdateData { get; set; } = new();

    public static string BinPath { get; set; } = string.Empty;
}
