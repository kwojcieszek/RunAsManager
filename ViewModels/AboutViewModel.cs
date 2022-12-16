using System.Windows;
using System.Windows.Input;

namespace RunAsManager.ViewModels
{
    public class AboutViewModel
    {
        public Window Window => Helper.GetWindow(this);

        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get => _closeCommand ??= new RelayCommand(
                  x =>
                  {
                      this.Window.Close();
                  });
        }
    }
}