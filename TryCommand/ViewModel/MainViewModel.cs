using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using TryCommand.Model;

using TryCommand.View;

namespace TryCommand.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private Action<string> _popup;
        public Action<string> Popup
        {
            get
            {
                return _popup;
            }
            set 
            {
                if (_popup != value)
                {
                    _popup = value;
                }
            }
        }
        private Func<string, string, bool> _confirm;
        public Func<string, string, bool> Confirm
        {
            get
            {
                return _confirm;
            }
            set
            {
                if (_confirm != value)
                {
                    _confirm = value;
                }
            }
        }

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            Popup = (Action<string>) (msg => MessageBox.Show(msg));
            Confirm = (Func<string, string, bool>)(
                (msg, capt) =>
                    MessageBox.Show(msg, capt, MessageBoxButton.YesNo) == MessageBoxResult.Yes);
        }

        private RelayCommand _clickMeCommand;

        /// <summary>
        /// Gets the ClickMeCommand.
        /// </summary>
        public RelayCommand ClickMeCommand
        {
            get
            {
                return _clickMeCommand
                    ?? (_clickMeCommand = new RelayCommand(ExecuteClickMeCommand));
            }
        }

        private void ExecuteClickMeCommand()
        {
            Popup("You clicked!");
        }

        private RelayCommand _quitCommand;

        /// <summary>
        /// Gets the QuitCommand.
        /// </summary>
        public RelayCommand QuitCommand
        {
            get
            {
                return _quitCommand
                    ?? (_quitCommand = new RelayCommand(ExecuteQuitCommand));
            }
        }

        private void ExecuteQuitCommand()
        {
            bool confirm = Confirm("Are you sure you want to exit", "confirm exit");
            if (confirm)
            {
                Application.Current.Shutdown();
            }
        }

        private RelayCommand _versionCommand;

        /// <summary>
        /// Gets the VersionCommand.
        /// </summary>
        public RelayCommand VersionCommand
        {
            get
            {
                return _versionCommand
                    ?? (_versionCommand = new RelayCommand(ExecuteVersionCommand));
            }
        }

        private void ExecuteVersionCommand()
        {
            var about = new About();
            about.ShowDialog();
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}