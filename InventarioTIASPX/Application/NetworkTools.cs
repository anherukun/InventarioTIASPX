using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace InventarioTIASPX.Application
{
    public class NetworkTools
    {
        public static string ResolveIPAddress(string hostname)
        {
            try
            {
                var host = Dns.GetHostAddresses(hostname);
                return host[0].ToString();
            }
            catch (Exception ex)
            {
                return "HOST NO DISPONIBLE";
            }
        }
    }
}