using Microsoft.AspNetCore.Mvc;
using PasswordMenager.Data;
using PasswordMenager.Models;
using PasswordMenager.Models.VModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Linq;

namespace PasswordMenager.Controllers
{
    public class ClientPasswordController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ClientPasswordController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ClientPasswordIndexVM> result;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                var passwordsList = _db.clientPasswords.ToList();

                result = passwordsList.Select(x => new ClientPasswordIndexVM() { 
                    Id = x.Id,
                    Password = decrypt(x.Password.Skip(16).ToArray() , sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(x.URI + x.UserName)).Take<byte>(16).ToArray(), x.Password.Take<byte>(16).ToArray()), 
                    URI = x.URI }).ToList();
            }
            return View(result);
        }
        public IActionResult Delete(int id)
        {
            _db.Remove(_db.clientPasswords.FirstOrDefault(x => x.Id == id));
            _db.SaveChanges();
            return Ok();
        }

        // z automatu get
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        // choni przed cross forgery
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClientPasswordVM obj)
        {

            string pass = obj.Password;
            byte[] encrypted;
            byte[] iv;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] key = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(obj.URI + User.Identity.Name));

                using (Aes myAes = Aes.Create())
                {
                    iv = myAes.IV;
                    encrypted = encrypt(pass, key.Take<byte>(16).ToArray(), iv);

                }
            }

            var cp = new ClientPassword() { 
                URI = obj.URI,
                Password = Combine(iv, encrypted),
                UserName = User.Identity.Name
            };

            _db.Add(cp);
            _db.SaveChanges();

            return View();
        }

        public byte[] encrypt(string plainText,byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public string decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

    }
}
