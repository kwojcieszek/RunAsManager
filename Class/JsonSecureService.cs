using Newtonsoft.Json;
using System.IO;

namespace RunAsManager
{
    public class JsonSecureService
    {
        private const int KEY = 432879324;

        public T Read<T>(string filePath) where T : class
        {
            var json = ReadFile(filePath);

            if (json == null)
                return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Write(object obj, string filePath)
        {
            var json = JsonConvert.SerializeObject(obj);

            WriteFile(json, filePath);
        }

        private string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var source = System.IO.File.ReadAllText(filePath);

            return Cryptography.XorEncryptDecrypt(source, KEY);
        }

        private void WriteFile(string source, string filePath)
        {
            source = Cryptography.XorEncryptDecrypt(source, KEY);

            var sw = new System.IO.StreamWriter(filePath);
            sw.Write(source);
            sw.Close();
        }
    }
}
