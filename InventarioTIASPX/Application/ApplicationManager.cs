using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Application
{
    public class ApplicationManager
    {
        public static string Base64Encode(string value)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }
        public static string Base64Decode(string value)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(value));
        }

        public static string GenerateGUID => Guid.NewGuid().ToString().ToUpper();
    }
}