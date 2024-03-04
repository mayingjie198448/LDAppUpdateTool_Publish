using System.Windows;
using LDAppUpdateTool_Publish.Modules.IndexView;
using LDAppUpdateTool_Publish.Modules.Log;
using LDAppUpdateTool_Publish.Modules.ModuleName;
using LDAppUpdateTool_Publish.Share.Common;
using LDAppUpdateTool_Publish.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace LDAppUpdateTool_Publish;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<ModuleNameModule>();
        moduleCatalog.AddModule<LogModule>();
        moduleCatalog.AddModule<IndexViewModule>();
    }

    protected override void OnInitialized()
    {
        if (Current.MainWindow is {DataContext: IConfigureService service})
            service.Configure();

        base.OnInitialized();
    }
}
