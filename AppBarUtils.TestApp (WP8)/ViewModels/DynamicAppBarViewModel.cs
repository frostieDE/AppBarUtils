//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using GalaSoft.MvvmLight.Command;

namespace AppBarUtils.TestApp.ViewModels
{
    public class DynamicAppBarViewModel : ViewModelBase
    {
        public DynamicAppBarViewModel()
        {
            SampleCommand = new ActionCommand(
                () =>
                {
                    MessageBox.Show("Sample command invoked.");
                });

            Todos = new ObservableCollection<TodoItem>(
                Enumerable.Range(1, 10)
                .Select(i =>
                    new TodoItem
                    {
                        Subject = "to-do #" + i,
                        DueDate = DateTime.Today.AddDays(i),
                    }));

            SelectCommand = new ActionCommand(
                () =>
                {
                    IsSelecting = true;
                });

            DeleteCommand = new ActionCommand(
                () =>
                {
                    MessageBox.Show("Delete command invoked.");
                    IsSelecting = false;
                });

            LockCommand = new RelayCommand(() => IsLocked = true, () => IsEnabled);
            UnlockCommand = new RelayCommand(() => IsLocked = false, () => IsEnabled);
            ToggleIsEnabledCommand = new RelayCommand(() => IsEnabled = !IsEnabled);
        }

        public ICommand SampleCommand { get; private set; }
        

        public ObservableCollection<TodoItem> Todos { get; private set; }

        private bool _isSelecting;
        public bool IsSelecting
        {
            get { return _isSelecting; }
            set
            {
                if (_isSelecting != value)
                {
                    _isSelecting = value;
                    RaisePropertyChanged("IsSelecting");
                }
            }
        }

        public ICommand SelectCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public RelayCommand LockCommand { get; private set; }
        public RelayCommand UnlockCommand { get; private set; }

        public RelayCommand ToggleIsEnabledCommand { get; private set; }

        public string AddButtonText
        {
            get { return "add"; }
        }

        public Uri AddButtonIcon
        {
            get { return new Uri("/icons/appbar.add.rest.png", UriKind.Relative); }
        }

        public string HelpMenuItemText
        {
            get { return "help"; }
        }

        private bool _isLocked = false;
        public bool IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (_isLocked != value)
                {
                    _isLocked = value;
                    RaisePropertyChanged("IsLocked");
                }
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }

                if (LockCommand != null)
                {
                    LockCommand.RaiseCanExecuteChanged();
                }

                if (UnlockCommand != null)
                {
                    UnlockCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }

    public class TodoItem
    {
        public string Subject { get; set; }
        public DateTime DueDate { get; set; }
    }
}
