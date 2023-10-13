using InvenTrack.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InvenTrack.ViewModel
{
    internal class InventoryAViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
            }
        }

        //Commands
        public ICommand ShowInventoryCommand { get; }
        public ICommand ShowOrdersCommand { get; }
        public ICommand ShowSuppliersCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand ShowSwitchCommand { get; }

        public InventoryAViewModel()
        {
            //Initialize commands
            ShowInventoryCommand = new ViewModelCommand(ExecuteShowInventoryCommand);
            ShowOrdersCommand = new ViewModelCommand(ExecuteShowOdersCommand);
            ShowSuppliersCommand = new ViewModelCommand(ExecuteShowSuppliersCommand);
            ShowReportsCommand = new ViewModelCommand(ExecuteShowReportsCommand);
            ShowSwitchCommand = new ViewModelCommand(ExecuteShowSwitchCommand);

            //Default view
            ExecuteShowInventoryCommand(null);
        }

        private void ExecuteShowInventoryCommand(object obj)
        {
            CurrentChildView = new AInventoryViewModel();
        }

        private void ExecuteShowOdersCommand(object obj)
        {
            CurrentChildView = new AOrdersViewModel();
        }
        private void ExecuteShowSuppliersCommand(object obj)
        {
            CurrentChildView = new ASuppliersViewModel();
        }
        private void ExecuteShowReportsCommand(object obj)
        {
            CurrentChildView = new AReportsViewModel();
        }
        private void ExecuteShowSwitchCommand(object obj)
        {
            CurrentChildView = new ASwitchViewModel();
        }
    }
}
