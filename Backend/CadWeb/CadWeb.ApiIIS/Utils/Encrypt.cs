using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CadWeb.ApiIIS.Utils
{
    public class Encrypt
    { 
        private string key ="!gohorse@2017";
        public Encrypt() { }
        public string RetornarMD5(string senha)

        {
            using (MD5 md5Hash = MD5.Create())
            {
                return RetonarHash(md5Hash, senha);
            }


            //byte[] SrctArray;

            //byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(Encryptval);

            //SrctArray = UTF8Encoding.UTF8.GetBytes(key);

            //TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

            //MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();

            //SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            //objcrpt.Clear();

            //objt.Key = SrctArray;

            //objt.Mode = CipherMode.ECB;

            //objt.Padding = PaddingMode.PKCS7;

            //ICryptoTransform crptotrns = objt.CreateEncryptor();

            //byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);

            //objt.Clear();

            //return Convert.ToBase64String(resArray, 0, resArray.Length);

        }

        //public string Decryptword(string DecryptText)

        //{


        //    //byte[] SrctArray;

        //    //byte[] DrctArray = Convert.FromBase64String(DecryptText);

        //    //SrctArray = UTF8Encoding.UTF8.GetBytes(key);

        //    //TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

        //    //MD5CryptoServiceProvider objmdcript = new MD5CryptoServiceProvider();

        //    //SrctArray = objmdcript.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

        //    //objmdcript.Clear();

        //    //objt.Key = SrctArray;

        //    //objt.Mode = CipherMode.ECB;

        //    //objt.Padding = PaddingMode.PKCS7;

        //    //ICryptoTransform crptotrns = objt.CreateDecryptor();

        //    //byte[] resArray = crptotrns.TransformFinalBlock(DrctArray, 0, DrctArray.Length);

        //    //objt.Clear();

        //    //return UTF8Encoding.UTF8.GetString(resArray);

        //}

        public bool ComparaMD5(string senhabanco, string Senha_MD5)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var senha = RetornarMD5(Senha_MD5);
                if (VerificarHash(md5Hash, senhabanco, senha))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private string RetonarHash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private bool VerificarHash(MD5 md5Hash, string input, string hash)
        {
            StringComparer compara = StringComparer.OrdinalIgnoreCase;

            if (0 == compara.Compare(input, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}