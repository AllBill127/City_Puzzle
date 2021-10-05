using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace CityPuzzle.Classes
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }

        public List<int> QuestsComlited { get; set; }
      
        public User() { }
        public static Boolean CheckPassword(string name, string pass)
        {
             if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(pass))
            {
                return false;
            }

            pass = EncryptPlainTextToCipherText(pass);                                   //encrypt
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<User>();
                //conn.DeleteAll<User>();
                var info = conn.Table<User>().ToList();
                
                App.CurrentUser = info.SingleOrDefault(x => x.UserName.ToLower().Equals(name.ToLower()) && x.Pass.ToLower().Equals(pass.ToLower()));
            
            };
            return App.CurrentUser != null;
        }
        public static Boolean CheckUser(string name)
        { 
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<User>();

                var info = conn.Table<User>().ToList();
                Console.WriteLine(info.Count);
                foreach (User n in info)
                {
                    if (n.UserName.ToLower().Equals(name.ToLower()))
                    {
                        return false;
                    }
                }

            };
            return true;
        }
        /*
        public static void EncryptPass(string pass)
        {
            var text = pass;

            var encryptedText = EncryptPlainTextToCipherText(text);
            var decryptedText = DecryptCipherTextToPlainText(encryptedText);


            Console.WriteLine("Passed Text = " + text);
            Console.WriteLine("EncryptedText = " + encryptedText);
            Console.WriteLine("DecryptedText = " + decryptedText);
            Console.ReadLine();
        }
        */
        private const string SecurityKey = "SecurityKey_1212";
        public static string EncryptPlainTextToCipherText(string PlainText)
        {
           
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
           
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            
            objTripleDESCryptoService.Key = securityKeyArray;
            
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        
        public static string DecryptCipherTextToPlainText(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

           
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            
            objTripleDESCryptoService.Key = securityKeyArray;
            
            objTripleDESCryptoService.Mode = CipherMode.ECB;
           
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }



}
