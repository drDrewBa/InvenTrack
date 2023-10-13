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

        public InventoryAViewModel()
        {
            //Initialize commands
            ShowInventoryCommand = new ViewModelCommand(ExecuteShowInventoryCommand);

            //Default view
            ExecuteShowInventoryCommand(null);
        }

        private void ExecuteShowInventoryCommand(object obj)
        {
            CurrentChildView = new AInventoryViewModel();
        }
    }
}
