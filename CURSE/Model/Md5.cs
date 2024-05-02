using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CURSE.Model
{
    public class Md5
    {
        //private Md5() { }
        //static Md5 instance;
        //public static Md5 Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //            instance = new Md5();
        //        return instance;
        //    }
        //}
        public static string HashPassword (string password)
        {
            {
                var bytes = Encoding.ASCII.GetBytes(password);
                StringBuilder result = new StringBuilder();
                using (var md5 = MD5.Create())
                using (var ms = new MemoryStream(bytes))
                {
                    var hash = md5.ComputeHash(ms);
                    foreach (var b in hash)
                        result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }
    }
}
