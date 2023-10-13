using InvenTrack.Model;
using InvenTrack.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InvenTrack.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        //Fields

        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;

        private bool _isViewVisible = true;

        //Properties
        public UserAccountModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();

            //OpenInventoryA = new ViewModelCommand(ExecuteOpenInventoryA);
            //OpenInventoryB = new ViewModelCommand(ExecuteOpenInventoryB);
        }


        //private void ExecuteOpenInventoryA(object obj)
        //{
        //    IsViewVisible = false;
        //}

        //private void ExecuteOpenInventoryB(object obj)
        //{
        //    IsViewVisible = false;
        //}

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.UserName;
                CurrentUserAccount.DisplayName = $"Hello, {user.Name} {user.LastName}!";
                CurrentUserAccount.ProfilePicture = null;
            } 
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
            }
        }

        //Commands
        //public ICommand OpenInventoryA { get; }
        //public ICommand OpenInventoryB { get; }
    }
}
