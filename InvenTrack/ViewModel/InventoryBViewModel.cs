using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InvenTrack.ViewModel
{
    internal class InventoryBViewModel : ViewModelBase
    {

        private ViewModelBase _currentChildView;

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

        //Commands
        public ICommand ShowInventoryCommand { get; }
        public ICommand ShowOrdersCommand { get; }
        public ICommand ShowSuppliersCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand ShowSwitchCommand { get; }

        public InventoryBViewModel()
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
            CurrentChildView = new BInventoryViewModel();
        }

        private void ExecuteShowOdersCommand(object obj)
        {
            CurrentChildView = new BOrdersViewModel();
        }
        private void ExecuteShowSuppliersCommand(object obj)
        {
            CurrentChildView = new BSuppliersViewModel();
        }
        private void ExecuteShowReportsCommand(object obj)
        {
            CurrentChildView = new BReportsViewModel();
        }
        private void ExecuteShowSwitchCommand(object obj)
        {
            CurrentChildView = new BSwitchViewModel();
        }
    }
}
