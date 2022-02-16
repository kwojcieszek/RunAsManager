using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace RunAsManager.DbModel
{
    public class ProgramDetail
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Arguments { get; set; }
        [JsonIgnore]
        public BitmapImage Image { get; set; }
    }
}