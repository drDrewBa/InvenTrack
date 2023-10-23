using InvenTrack.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace InvenTrack.ViewModel
{
    public interface IBInventoryViewModel
    {
        ICommand AddCommand { get; }
        ICommand ClearCommand { get; }
        ICommand DeleteCommand { get; }
        ObservableCollection<InventoryItemModel> InventoryItems { get; set; }
        InventoryItemModel SelectedInventoryItem { get; set; }
        ICommand UpdateCommand { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}