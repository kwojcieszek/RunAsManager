using RunAsManager.DbModel;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace RunAsManager.Models
{
    internal class MainWindowModel
    {
        private readonly DbContext _dbContext = new();
        public ObservableCollection<ProgramDetail> ProgramsDetails { get; private set; }

        public MainWindowModel()
        {
            this.ProgramsDetails = this._dbContext.ProgramsDetails;
        }

        public void AddProgramDetail(string filePath, string name)
        {
            this.ProgramsDetails.Add(new ProgramDetail()
            {
                ID = Guid.NewGuid().ToString(),
                Name = name ?? Path.GetFileNameWithoutExtension(filePath),
                Arguments = String.Empty,
                Path = filePath,
                Image = ImageHelper.ToBitmapImage(ImageHelper.ImageToByte(ImageHelper.IconFromFilePath(filePath)))
            });

            this.Save();
        }

        public void Save()
        {
            this._dbContext.Save();
        }

        public void Delete(ProgramDetail programDetail)
        {
            ProgramsDetails.Remove(programDetail);

            this.Save();
        }

        public void CreateLinkOnDesktop(ProgramDetail programDetail)
        {
            var arguments = $"\"{programDetail.Path}\" \"{programDetail.Arguments}\"";

            new WindowsLink().Create(
                programDetail.Name,
                arguments,
                String.Empty,
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                System.Reflection.Assembly.GetExecutingAssembly().Location,
                System.IO.Path.GetDirectoryName(programDetail.Path),
                programDetail.Path);
        }
    }
}