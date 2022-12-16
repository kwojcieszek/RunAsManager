using System;
using System.Collections.ObjectModel;

namespace RunAsManager.DbModel
{
    internal class DbContext
    {
        public ObservableCollection<ProgramDetail> ProgramsDetails { get; private set; }
        public string AdministratorUserName { get; set; }
        private readonly string _filePath;
        private readonly JsonSecureService _json = new();

        public DbContext(string filePath = null)
        {
            if (filePath == null)
                this._filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\RunAsManagerDb";
            else
                this._filePath = filePath;

            this.Load();
        }

        private void Load()
        {
            var dbContextData = this._json.Read<DbContextData>(this._filePath);

            if (dbContextData == null)
            {
                dbContextData = new()
                {
                    ProgramsDetails = new(),
                    AdministratorUserName = string.Empty
                };
            }

            this.ProgramsDetails = dbContextData.ProgramsDetails;
            this.AdministratorUserName = dbContextData.AdministratorUserName;

            foreach (var program in this.ProgramsDetails)
                program.Image = ImageHelper.ToBitmapImage(ImageHelper.ImageToByte(ImageHelper.IconFromFilePath(program.Path)));
        }

        public void Save()
        {
            var dbContextData = new DbContextData()
            {
                ProgramsDetails = this.ProgramsDetails,
                AdministratorUserName = this.AdministratorUserName,
            };

            this._json.Write(dbContextData, this._filePath);
        }

        private class DbContextData
        {
            public ObservableCollection<ProgramDetail> ProgramsDetails { get; set; }
            public string AdministratorUserName { get; set; }
        }
    }
}