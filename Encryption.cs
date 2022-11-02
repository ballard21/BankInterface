using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JobAssignment
{
    public class Encryption
    {
        private string encrypted;
        private string decrypted;
        private string password = "";
        private TripleDESCryptoServiceProvider des;
        private MD5CryptoServiceProvider hashmd5;
        private byte[] pwdhash;
        private byte[] buff;
        public Encryption()
        {
            password = "PleaseBreakIt";
            hashmd5 = new MD5CryptoServiceProvider();
            pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            hashmd5 = null;
            des = new TripleDESCryptoServiceProvider
            {
                Key = pwdhash,
                Mode = CipherMode.ECB
            };
        }

        public string EncryptString(string Original)
        {
            try
            {
                buff = ASCIIEncoding.ASCII.GetBytes(Original);
                encrypted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
            }
            catch (Exception eDecr)
            {
                string x = eDecr.Message;
                //MessageBox.Show(eDecr.Message.ToString(),"Encryption");
                //Application.Exit();
            }
            return encrypted;
        }

        public string DecryptString(string EncryptedText)
        {
            try
            {
                buff = Convert.FromBase64String(EncryptedText);
                decrypted = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
            }
            catch (Exception eDecr)
            {
                string x = eDecr.Message;
                //MessageBox.Show(eDecr.Message.ToString(),"Encryption");
                //Application.Exit();
            }
            return decrypted;
        }

        public string getOracleConnString(string OracleIP, string OraclePort, string OracleServer, string OracleSID, string OracleUser, string OraclePassword)
        {
            string OracleConnString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + OracleIP + ")(PORT=" + OraclePort + "))" +
                "(CONNECT_DATA=(SERVER=" + OracleServer + ")(SERVICE_NAME=" + OracleSID + ")));" +
                "User Id=" + OracleUser + ";Password=" + OraclePassword + ";";
            return OracleConnString;
        }
    }
}