using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class ReporterService
    {
        /// <summary>
        /// Crea un reporte a partir de una <see cref="List{T}"/> y lo devuelve en valores separados por un caracter determinado
        /// </summary>
        /// <param name="computers">Lista de <see cref="Computer"/></param>
        /// <param name="separator">Caracter delimitador</param>
        /// <returns></returns>
        public static string GetComputersReport(List<Computer> computers, char s)
        {
            string result = "";
            // DEPARTMENT , LOCATION , COMPUTER_TYPE , COMPUTERID , HOSTNAME , SYSTEM_ARC , JOBCATEGORY , IP_ADDRESS , MAC_ADDRESS , AD_ACCOUNT , ACCOUNT_NAME , ACCOUNT_OWNER , ACCOUNT_EMAIL , ATTACHED_DEVICES 
            result = $"DEPARTMENT{s}LOCATION{s}COMPUTER_TYPE{s}COMPUTER_MODEL{s}COMPUTERID{s}HOSTNAME{s}SYSTEM_ARC{s}JOBCATEGORY{s}IP_ADDRESS{s}MAC_ADDRESS{s}AD_ACCOUNT{s}ACCOUNT_NAME{s}ACCOUNT_OWNER{s}ACCOUNT_EMAIL{s}ATTACHED_DEVICE_1{s}ATTACHED_DEVICE_2{s}ATTACHED_DEVICE_3{s}ATTACHED_DEVICE_4{s}ATTACHED_DEVICE_5\n";
            foreach (var c in computers)
            {
                User u = c.UserGUID != null ? RepositoryUser.Get(c.UserGUID) : null;
                List<Device> devices = RepositoryComputer.Get(c.ComputerId).Devices;
                string ipaddress = Application.NetworkTools.ResolveIPAddress(c.Hostname) != null ? Application.NetworkTools.ResolveIPAddress(c.Hostname) : null;
                //string ipaddress = null;

                string line = $"{c.Department}{s}{c.Location.Replace(",", "")}{s}{c.Processor.Type.Replace("PROCESADOR","DESKTOP")}{s}{c.Processor.Brand} - {c.Processor.Model}{s}{c.ComputerId}{s}{c.Hostname}{s}{c.Architecture} BITS{s}" +
                    $"{c.JobCategory}{s}IP_ADDRESS{s}MAC_ADDRESS{s}AD_ACCOUNT{s}ACCOUNT_NAME{s}ACCOUNT_OWNER{s}ACCOUNT_EMAIL ATTACHED_DEVICES \n";

                if (ipaddress != null)
                {
                    Dictionary<string, List<string>> values = Application.NetworkTools.LegacyGetMacAddress(ipaddress);
                    
                    line = line.Replace("IP_ADDRESS", ipaddress);
                    
                    if (values != null && values.ContainsKey($"{ipaddress}"))
                        line = line.Replace("MAC_ADDRESS", values[ipaddress][0].ToUpper());
                    else
                        line = line.Replace("MAC_ADDRESS", "----");
                }
                else
                {
                    line = line.Replace("IP_ADDRESS", "----");
                    line = line.Replace("MAC_ADDRESS", "----");
                }

                if (devices != null)
                {
                    string d = "";
                    foreach (var item in devices)
                    {
                        if (item.Type != "PROCESADOR" && item.Type != "LAPTOP") 
                            d += $"{s}{item.Type} - {item.DeviceId}";
                    }
                    line = line.Replace("ATTACHED_DEVICES", d);
                }
                else
                {
                    line = line.Replace("ATTACHED_DEVICES", "----");
                }

                if (u != null)
                {
                    line = line.Replace("AD_ACCOUNT", u.UserId.ToString());
                    line = line.Replace("ACCOUNT_NAME", u.Name);
                    line = line.Replace("ACCOUNT_OWNER", u.Employe);
                    line = line.Replace("ACCOUNT_EMAIL", u.Email);
                } 
                else
                {
                    line = line.Replace("AD_ACCOUNT", "----");
                    line = line.Replace("ACCOUNT_NAME", "----");
                    line = line.Replace("ACCOUNT_OWNER", "----");
                    line = line.Replace("ACCOUNT_EMAIL", "----");
                }

                result += line;
            }

            return result;
        }

        /// <summary>
        /// Crea un reporte a partir de una <see cref="List{T}"/> y lo devielve en valores separados por comas
        /// </summary>
        /// <param name="printers">Lista de impresoras</param>
        /// <param name="s">Simbolo delimitador</param>
        /// <returns></returns>
        public static string GetPrintersReport(List<Printer> printers, char s)
        {
            string result = "";
            // DEPARTMENT , LOCATION , PRINTERID , PRINTER_MODEL , CONNECTION_TYPE , HOSTNAME , IP_ADDRESS , MAC_ADDRESS , OWNER_ID , OWNER_NAME , OWNER_MAIL 
            result = $"DEPARTMENT{s}LOCATION{s}PRINTERID{s}PRINTER_MODEL{s}CONNECTION_TYPE{s}HOSTNAME{s}IP_ADDRESS{s}MAC_ADDRESS{s}OWNER_ID{s}OWNER_NAME{s}OWNER_MAIL\n";
            foreach (var p in printers)
            {
                User u = p.UserGUID != null ? RepositoryUser.Get(p.UserGUID) : null;
                string ipaddress = "----";
                if (p.ConnectionType == "RED")
                    ipaddress = Application.NetworkTools.ResolveIPAddress(p.Hostname) != null ? Application.NetworkTools.ResolveIPAddress(p.Hostname) : null;

                string line = $"{p.Department}{s}{p.Location.Replace(",", "")}{s}{p.PrinterId}{s}{p.Brand} - {p.Model}{s}{p.ConnectionType}{s}{p.Hostname}{s}IP_ADDRESS{s}MAC_ADDRESS{s}OWNER_ID{s}OWNER_NAME{s}OWNER_MAIL\n";

                if (ipaddress != "----")
                {
                    Dictionary<string, List<string>> values = Application.NetworkTools.LegacyGetMacAddress(ipaddress);

                    line = line.Replace("IP_ADDRESS", ipaddress);

                    if (values != null && values.ContainsKey($"{ipaddress}"))
                        line = line.Replace("MAC_ADDRESS", values[ipaddress][0].ToUpper());
                    else
                        line = line.Replace("MAC_ADDRESS", "----");
                }
                else
                {
                    line = line.Replace("IP_ADDRESS", "----");
                    line = line.Replace("MAC_ADDRESS", "----");
                }

                if (u != null)
                {
                    line = line.Replace("OWNER_ID", u.EmployeId.ToString());
                    line = line.Replace("OWNER_NAME", u.Employe);
                    line = line.Replace("OWNER_MAIL", u.Email);
                }
                else
                {
                    line = line.Replace("OWNER_ID", "----");
                    line = line.Replace("OWNER_NAME", "----");
                    line = line.Replace("OWNER_MAIL", "----");
                }

                result += line;
            }

            return result;
        }

        /// <summary>
        /// Crea un reporte a partir de una <see cref="List{T}"/> y lo devuelve en valores separados por comas
        /// </summary>
        /// <param name="users">Lista de <see cref="User"/></param>
        /// <param name="s">Caracter delimitador</param>
        /// <returns></returns>
        public static string GetUsersReport(List<User> users, char s)
        {
            string result = "";
            // ACTIVEDIRECTORY_ID , ACCOUNT_NAME , ACCOUNT_EMAIL , EMPLOYE_ID , EMPLOYE_NAME , OFFICE365_MIGRATED 
            result = $"ACTIVEDIRECTORY_ID{s}ACCOUNT_NAME{s}ACCOUNT_EMAIL{s}EMPLOYE_ID{s}EMPLOYE_NAME{s}OFFICE365_MIGRATED\n";
            foreach (var u in users)
            {
                string line = $"{u.UserId}{s}{u.Name}{s}{u.Email}{s}{u.EmployeId}{s}{u.Employe}{s}OFFICE365_MIGRATED\n";

                if (u.Migrated)
                    line = line.Replace("OFFICE365_MIGRATED", "SI");
                else
                    line = line.Replace("OFFICE365_MIGRATED", "NO");

                result += line;
            }

            return result;
        }
    }
}