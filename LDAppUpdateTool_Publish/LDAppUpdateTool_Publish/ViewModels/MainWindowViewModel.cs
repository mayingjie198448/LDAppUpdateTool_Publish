using LDAppUpdateTool_Publish.Share.Common;
using Prism.Mvvm;

namespace LDAppUpdateTool_Publish.ViewModels;

public class MainWindowViewModel : BindableBase, IConfigureService
{
    private string _title = "LDAppUpdateTool_Publish";

    public string Title {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public void Configure()
    { }
}
