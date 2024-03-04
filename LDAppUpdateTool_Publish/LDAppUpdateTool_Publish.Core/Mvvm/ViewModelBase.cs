using Prism.Mvvm;
using Prism.Navigation;

namespace LDAppUpdateTool_Publish.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
