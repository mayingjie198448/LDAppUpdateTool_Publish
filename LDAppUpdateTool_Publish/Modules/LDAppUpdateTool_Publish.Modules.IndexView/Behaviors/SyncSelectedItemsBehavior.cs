using System.Collections.ObjectModel;
using System.Windows.Controls;
using LDAppUpdateTool_Publish.Modules.IndexView.ViewModels;
using Microsoft.Xaml.Behaviors;

namespace LDAppUpdateTool_Publish.Modules.IndexView.Behaviors;

public class SyncSelectedItemsBehavior : Behavior<ListBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += OnSelectionChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.SelectionChanged -= OnSelectionChanged;
        base.OnDetaching();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        var selectedItems = listBox?.SelectedItems;
        var viewModel = listBox?.DataContext as IndexViewModel;
        var targetCollection = viewModel?.SelectedFiles;
        foreach (var item in e.RemovedItems)
        {
            targetCollection?.Remove(item as string);
        }

        foreach (var item in e.AddedItems)
        {
            targetCollection?.Add(item as string);
        }
    }
}
