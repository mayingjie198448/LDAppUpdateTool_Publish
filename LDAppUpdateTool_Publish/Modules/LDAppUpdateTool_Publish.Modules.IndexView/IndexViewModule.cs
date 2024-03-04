using LDAppUpdateTool_Publish.Core;
using LDAppUpdateTool_Publish.Services;
using LDAppUpdateTool_Publish.Services.InterFaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace LDAppUpdateTool_Publish.Modules.IndexView;

public class IndexViewModule : IModule
{
    private readonly IRegionManager _regionManager;

    public IndexViewModule(IRegionManager regionManager)
    {
        _regionManager = regionManager;
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        _regionManager.RegisterViewWithRegion(RegionNames.IndexRegion, typeof(Views.IndexView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<Views.IndexView>();
        containerRegistry.Register<IUpdateService, UpdateService>();
    }
}
