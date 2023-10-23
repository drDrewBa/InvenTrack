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
    public class AOrdersViewModel : ViewModelBase
    {
    }

    public class OrdersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<OrderItemModel> orderItems;
        private OrderItemModel selectedOrderItem;
        private decimal totalSum;
        private decimal receivedValue;
        private decimal change;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<OrderItemModel> OrderItems
        {
            get { return orderItems; }
            set
            {
                orderItems = value;
            }
        }

        public OrderItemModel SelectedOrderItem
        {
            get { return selectedOrderItem; }
            set
            {
                selectedOrderItem = value;
            }
        }

        public decimal TotalSum
        {
            get { return totalSum; }
            set
            {
                totalSum = value;
            }
        }

        public decimal ReceivedValue
        {
            get { return receivedValue; }
            set
            {
                receivedValue = value;
            }
        }

        public decimal Change
        {
            get { return change; }
            set
            {
                change = value;
            }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CompleteCommand { get; }

        public OrdersViewModel()
        {
            // Initialize commands and load data
            // You can define your commands (AddCommand, RemoveCommand, CancelCommand, CompleteCommand) here.
        }

        // Implement your command logic here.
        // Example:
        private void ExecuteAddCommand()
        {
            // Add order item logic
        }

        // Implement INotifyPropertyChanged interface
    }
}
