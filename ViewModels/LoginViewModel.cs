using RunAsManager.Models;
using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace RunAsManager.ViewModels
{
    public class LoginViewModel
    {
        private readonly LoginModel _model = new();
        public Window Window => Helper.GetWindow(this);
        public string UserName { get; set; }
        public string Password { get; set; }
        private ICommand _loginCommand;
        private ICommand _cancelCommand;

        public ICommand LoginCommand
        {
            get => _loginCommand ??= new RelayCommand(
                  x =>
                  {
                      try
                      {
                          if (Environment.UserName == this.UserName)
                          {
                              MessageBox.Show("Login name and current user are the same!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                              return;
                          }

                          this.LoginUser(this.UserName, this.Password);

                          this._model.AdministratorUserName = this.UserName;
                          this._model.Password = this.Password;

                          this._model.Save();

                          this.Window.DialogResult = true;
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                      }
                  });
        }

        public ICommand CancelCommand
        {
            get => _cancelCommand ??= new RelayCommand(
                  x =>
                  {
                      this.Window.Close();
                  });
        }

        public LoginViewModel()
        {
            this.UserName = this._model.AdministratorUserName;
        }

        public void LoginUser(string username, string password)
        {
            var secureString = new SecureString();

            foreach (var p in password)
                secureString.AppendChar(p);

            new RunAs().Start(username, secureString, System.Reflection.Assembly.GetExecutingAssembly().Location, "login_test");

            App.CreateToken();
        }
    }
}