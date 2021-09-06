using System;
using System.IO;
using System.Security.Cryptography;
using API_Password_Generator.Models.InputModels;
using API_Password_Generator.Models.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API_Password_Generator.Models.Services.Application
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private readonly ILogger<PasswordGeneratorService> logger;
        private readonly IOptionsMonitor<SecurityOptions> securityOptionsMonitor;

        public PasswordGeneratorService(ILogger<PasswordGeneratorService> logger, IOptionsMonitor<SecurityOptions> securityOptionsMonitor)
        {
            this.logger = logger;
            this.securityOptionsMonitor = securityOptionsMonitor;
        }

        //Customized code starting from the example: https://gist.github.com/RichardHan/0848a25d9466a21f1f38
        public string PasswordGeneratorAsync(InputPassword model, IWebHostEnvironment env)
        {
            var options = this.securityOptionsMonitor.CurrentValue;

            string EncryptKey_1 = options.EncryptKey_1;
            string EncryptKey_2 = options.EncryptKey_2;

            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(EncryptKey_1);
                aes.IV = Convert.FromBase64String(EncryptKey_2);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(model.Password);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string PasswordRestoreAsync(InputPassword model, IWebHostEnvironment env)
        {
            var options = this.securityOptionsMonitor.CurrentValue;

            string EncryptKey_1 = options.EncryptKey_1;
            string EncryptKey_2 = options.EncryptKey_2;

            string decrypted = null;
            byte[] cipher = Convert.FromBase64String(model.Password);

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Convert.FromBase64String(EncryptKey_1);
                aes.IV = Convert.FromBase64String(EncryptKey_2);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }
    }
}