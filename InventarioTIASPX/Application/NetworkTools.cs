using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace InventarioTIASPX.Application
{
    public class NetworkTools
    {
        public static string ResolveIPAddress(string hostname)
        {
            try
            {
                // var host = Dns.GetHostAddresses(hostname);
                // return host[0].ToString();

                IPHostEntry hostEntry;
                hostEntry = Dns.GetHostEntry(hostname);

                if (hostEntry.AddressList.Length > 0)
                {
                    return hostEntry.AddressList.Where(o => o.AddressFamily == AddressFamily.InterNetwork).ToArray()[0].ToString();
                    // var ip = hostEntry.AddressList[0];
                }
                else throw new Exception();
            }
            catch (Exception ex)
            {
                return "HOST NO DISPONIBLE";
            }
        }

        public static PingReply MakePing(string hostname)
        {
            try
            {
                Ping sender = new Ping();
                PingOptions options = new PingOptions();

                options.DontFragment = true;
                string data = "Pinging to remote system";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                int timeout = 120;
                
                return sender.Send(hostname, timeout, buffer, options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Dictionary<string, string> LegacyGetMacAddress(string ipaddress)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "arp.exe";
            p.StartInfo.Arguments = "-a";
            p.Start();

            string result = p.StandardOutput.ReadToEnd();
            result = result.Replace("\r", "").Trim();
            result = result.Replace("\n ", "\n").Trim();
            string newstring = Regex.Replace(result, " {2,}", ",");

            p.WaitForExit();

            if (newstring.Trim().Split('\n').Length >= 3)
            {
                newstring = newstring.Split('\n')[3];

                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("ipaddress", newstring.Split(',')[0].Trim());
                values.Add("macaddress", newstring.Split(',')[1].Trim());
                values.Add("type", newstring.Split(',')[2].Trim());

                return values;
            } else
            {
                return null;
            }
        }
    }
}