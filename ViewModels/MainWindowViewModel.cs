using Microsoft.Win32;
using RunAsManager.DbModel;
using RunAsManager.Models;
using RunAsManager.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RunAsManager.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowModel _model = new();
        public ObservableCollection<ProgramDetail> ProgramsDetails => this._model.ProgramsDetails;
        public Window Window => Helper.GetWindow(this);
        public ProgramDetail SelectedItem { get; set; }
        public bool IsAdminMode => App.IsToken;
        public string Title => IsAdminMode ? $"RunAs Manager [Administrator mode]" : $"RunAs Manager";
        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand DropCommand { get; set; }
        private ICommand _createLinkCommand;
        private ICommand _deleteCommand;
        private ICommand _loginCommand;
        private ICommand _aboutCommand;
        public ICommand _addFileCommand;
        public ICommand _updateCommand;

        public ICommand CreateLinkCommand
        {
            get => _createLinkCommand ??= new RelayCommand(
                   x =>
                   {
                       if (this.SelectedItem != null && App.IsToken)
                           this._model.CreateLinkOnDesktop(this.SelectedItem);
                   });
        }

        public ICommand DeleteCommand
        {
            get => _deleteCommand ??= new RelayCommand(
                   x =>
                   {
                       if (this.SelectedItem != null && App.IsToken)
                       {
                           this._model.Delete(this.SelectedItem);

                           OnPropertyChange(nameof(ProgramsDetails));
                       }
                   });
        }

        public ICommand LoginCommand
        {
            get => _loginCommand ??= new RelayCommand(
                   x =>
                   {
                       var view = new LoginView
                       {
                           Owner = this.Window
                       };
                       view.ShowDialog();

                       this.OnPropertyChange(nameof(Title));
                       this.OnPropertyChange(nameof(IsAdminMode));
                   });
        }

        public ICommand AboutCommand
        {
            get => _aboutCommand ??= new RelayCommand(
                   x =>
                   {
                       var view = new AboutView
                       {
                           Owner = this.Window
                       };
                       view.ShowDialog();
                   });
        }

        public ICommand AddFileCommand
        {
            get => _addFileCommand ??= new RelayCommand(
                   x =>
                   {
                       var op = new OpenFileDialog
                       {
                           Title = "SELECT FILE",
                           Filter = "EXE|*.exe"
                       };

                       if (op.ShowDialog() == true)
                       {
                           this.AddProgram(op.FileName);
                           this.OnPropertyChange(nameof(ProgramsDetails));
                       }
                   });
        }

        public ICommand UpdateCommand
        {
            get => _updateCommand ??= new RelayCommand(
                   x =>
                   {
                       if (this.IsAdminMode)
                           this._model.Save();
                   });
        }

        public MainWindowViewModel()
        {
            this.GridDoubleClickCommand = new RelayCommand(GridDoubleClick);

            this.DropCommand = new RelayCommand(Drop);
        }

        public void GridDoubleClick(object obj)
        {
            if (this.SelectedItem != null)
                new RunService().Run(SelectedItem);
        }

        public void Drop(object obj)
        {
            if (obj is not IDataObject e)
                return;

            if (e.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.GetData(DataFormats.FileDrop);

                if (files.Length == 0)
                    return;
                this.AddProgram(files[0]);
            }
        }

        public void AddProgram(string filePath)
        {
            string name = null;

            if (!App.IsToken)
                return;

            if (filePath.Contains(".lnk"))
                (filePath, name) = new WindowsLink().GetShortcutTargetFile(filePath);

            this._model.AddProgramDetail(filePath, name);

            OnPropertyChange(nameof(ProgramsDetails));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
