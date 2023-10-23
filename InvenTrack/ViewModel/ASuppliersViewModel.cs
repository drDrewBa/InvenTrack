using InvenTrack.Model;
using InvenTrack.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows;

namespace InvenTrack.ViewModel
{
    public class ASuppliersViewModel : ViewModelBase
    {
        private ObservableCollection<ContactModel> contacts;
        private ContactModel selectedContact;

        public ObservableCollection<ContactModel> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                OnPropertyChanged("Contacts");
            }
        }

        public ContactModel SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                OnPropertyChanged("SelectedContact");
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        // Implement your command logic here.
        // Example:
        private void ExecuteAddCommand()
        {
            // Add contact logic
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
