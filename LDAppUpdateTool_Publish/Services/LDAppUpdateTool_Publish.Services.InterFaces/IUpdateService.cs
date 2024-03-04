namespace LDAppUpdateTool_Publish.Services.InterFaces;

public interface IUpdateService
{
    // 把要添加的文件生成压缩文件
    public void CreateAddOrUpdateZipFile(string zipPath);

    // 生成更新策略配置文件
    public void CreateUpdateConfigFile(string path);

    // 使用压缩文件生成hash值
    public string GetZipFileHash(string zipPath);

}
