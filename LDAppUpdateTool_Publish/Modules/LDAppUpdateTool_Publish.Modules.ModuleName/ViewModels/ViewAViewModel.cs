using LDAppUpdateTool_Publish.Core.Mvvm;
using Prism.Regions;

namespace LDAppUpdateTool_Publish.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewAViewModel(IRegionManager regionManager) :
            base(regionManager)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
