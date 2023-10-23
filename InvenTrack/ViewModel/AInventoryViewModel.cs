using InvenTrack.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InvenTrack.ViewModel
{
    public class AInventoryViewModel : ViewModelBase
    {

    }

    public class InventoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<InventoryItemModel> inventoryItems;
        private InventoryItemModel selectedInventoryItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<InventoryItemModel> InventoryItems
        {
            get { return inventoryItems; }
            set
            {
                inventoryItems = value;
            }
        }

        public InventoryItemModel SelectedInventoryItem
        {
            get { return selectedInventoryItem; }
            set
            {
                selectedInventoryItem = value;
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public InventoryViewModel()
        {
            // Initialize commands and load data
            // You can define your commands (AddCommand, UpdateCommand, DeleteCommand, ClearCommand) here.
        }

        // Implement your command logic here.
        // Example:
        private void ExecuteAddCommand()
        {
            // Add inventory item logic
        }

        // Implement INotifyPropertyChanged interface
    }
}
