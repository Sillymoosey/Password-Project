using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq.Expressions;


/// <summary>
/// This project was colab with Phill, Jenny, Jack, Gus, and Angel!
/// 
/// </summary>

namespace PasswordEncryption
{
    class PasswordEncryption
    {
        static void Main(string[] args)
        {
            MainMenu();
        }
        static void MainMenu()
        {
            bool isActive = true;
            string userinput;
            while (isActive)
            {
                Console.Clear();
                Console.WriteLine("1) Create an account type" +
                                "\n2) Authenticate a user" +
                               " \n3) Exit");
                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1":
                        CreateAccount();
                        isActive = false;
                       
                        break;
                    case "2":
                        AuthUser();
                        isActive = false;
                        
                        break;
                    case "3":
                        ProtectedData.print();
                        isActive = false;
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        static void CreateAccount()
        {
            string userName;
            string password;
            string encryptedPass;
            Console.Write("Username:");
            userName = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Password");
            password = Console.ReadLine();
            
            encryptedPass = ProtectedData.Encrypt(password);
            ProtectedData.Decrypt(encryptedPass);
            Console.WriteLine($"Your password is {password}");
            Console.WriteLine($"Your encryption is {encryptedPass}");

            // Store User In Dictionary 
            ProtectedData.StoreUser(userName, encryptedPass);
            Console.WriteLine("Account Created");
            Console.ReadKey();
            MainMenu();
        }

        public static void AuthUser()
        {
            string username;
            string password;
            Console.Write("UserName:");
            username = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Password:");
            password = Console.ReadLine();
            ProtectedData.CheckUser(username, password);
            Console.ReadKey();
            MainMenu();
        }

    }

     class ProtectedData
    {
        static Dictionary<string, string> users = new Dictionary<string, string>();

        static string hash = "f0xle@rn";
        public static string Encrypt(string userPass)
        {

            //Convert string into Bytes
            byte[] userBytePass = UTF8Encoding.UTF8.GetBytes(userPass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(userBytePass, 0, userBytePass.Length);
                    //Console.WriteLine($"Encrypted Pass: {Convert.ToBase64String(results, 0, results.Length)}");
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }

        }

        public static void Decrypt(string encryptedPass)
        {
            //Convert string into Bytes
            byte[] userBytePass = Convert.FromBase64String(encryptedPass);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDES.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(userBytePass, 0, userBytePass.Length);
                    Console.WriteLine(UTF8Encoding.UTF8.GetString(results));
                }
            }
        }
         
        public static void StoreUser(string userName, string encrytedPass)
        {
            // add user to dictionary
            users.Add(userName, encrytedPass);
        }
        public static void CheckUser(string userName, string encryptedPass)
        {
            encryptedPass = Encrypt(encryptedPass);
            bool cheapTrick = false;

            foreach (KeyValuePair<string, string> item in users)
            {
                if(item.Key == userName && item.Value == encryptedPass)
                {
                    cheapTrick = true;
                    break;
                }
            }

            if (cheapTrick)
            {
                Console.WriteLine("Authenticated!!!!!!!!!!!!!!!!!!!!!!!");
            }
            else
            {
                Console.WriteLine("Wrong! go to jail!");
            }
        }
        public static void print()
        {
            foreach (var item in users)
            {
                Console.WriteLine(item);
            }
        }
    }
}