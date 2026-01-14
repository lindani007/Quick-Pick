using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SecurityApi
{
    public class CodeGenarator
    {
        public static string CreateCode()
        {
            int code = RandomNumberGenerator.GetInt32(1000, 10000);
            return code.ToString();
        }
    }
}
