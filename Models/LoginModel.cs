using RunAsManager.DbModel;

namespace RunAsManager.Models
{
    internal class LoginModel
    {
        private readonly DbContext _dbContext = new();
        public string Password { get; set; }
        public string AdministratorUserName
        {
            get => this._dbContext.AdministratorUserName;
            set => this._dbContext.AdministratorUserName = value;
        }

        public void Save()
        {
            var passwordService = new PasswordService();

            passwordService.StorePassword(this.AdministratorUserName, this.Password);

            this._dbContext.Save();
        }
    }
}