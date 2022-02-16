using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace RunAsManager
{
    internal class PasswordService
    {
        private readonly string RegKey = $"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Fax\\{Cryptography.CreateMD5Hash(Environment.UserName)}";

        private string GetUserLowDirectory()
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{Cryptography.CreateMD5Hash(Environment.UserName)}";
        }

        private int GetCode()
        {
            return 89378478;
        }

        private byte[] GetKey()
        {
            var directory = $"{this.GetUserLowDirectory()}\\";
            var file = $"{directory}\\{Cryptography.CreateMD5Hash("KEY")}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(file))
                return File.ReadAllBytes(file);
            else
            {
                var key = this.GenerateKey();

                File.WriteAllBytes(file, key);

                return key;
            }
        }

        private byte[] GetIV()
        {
            var directory = $"{this.GetUserLowDirectory()}\\";
            var file = $"{directory}\\{Cryptography.CreateMD5Hash("IV")}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(file))
                return File.ReadAllBytes(file);
            else
            {
                var key = this.GenerateKey();

                File.WriteAllBytes(file, key);

                return key;
            }
        }

        private byte[] GenerateKey()
        {
            var key = new List<byte>();
            var rand = new Random();

            for (int i = 0; i < 16; i++)
                key.Add((byte)rand.Next(0, 255));

            return key.ToArray();
        }

        public void StorePassword(string userName, string passowrd)
        {
            var key = this.GetKey();

            var iv = this.GetIV();

            var passwordKey = Cryptography.EncryptStringToBytes(Cryptography.XorEncryptDecrypt(passowrd, this.GetCode()), key, iv);

            Registry.SetValue(RegKey, $"{Cryptography.CreateMD5Hash(userName)}", passwordKey);
        }

        public SecureString? GetPassword(string userName)
        {
            var key = this.GetKey();

            var iv = this.GetIV();

            if (Registry.GetValue(RegKey, $"{Cryptography.CreateMD5Hash(userName)}", null) is not byte[] passwordCrypt)
                return null;

            var password = Cryptography.XorEncryptDecrypt(Cryptography.DecryptStringFromBytes(passwordCrypt, key, iv), this.GetCode());

            var secureString = new SecureString();

            foreach (var p in password)
                secureString.AppendChar(p);

            return secureString;
        }
    }
}