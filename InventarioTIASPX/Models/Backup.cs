using InventarioTIASPX.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Services.Description;

namespace InventarioTIASPX.Models
{
    [Serializable()]
    public class Backup
    {
        // ENTIDAD PRINCIPAL
        public List<Device> Devices { get; set; }
        // ENTIDAD PRINCIPAL
        public List<User> Users { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Printer> Printers { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Computer> Computers { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Note> Notes { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<FileObject> Files { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<UserMemberOf> MemberOfs { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Dictionary<string, string>> ComputerDeviceRelationship { get; set; }
        public List<Dictionary<string, string>> UserMemberOfRelationship { get; set; }

        /// <summary>
        /// Crea un objeto de recuperacion
        /// </summary>
        public Backup()
        {
        }
        /// <summary>
        /// Crea un objeto de recuperacion a partir de los objetos enumerados
        /// </summary>
        /// <param name="devices">Objeto lista de dispositivos</param>
        /// <param name="users">Objeto lista de usuarios</param>
        /// <param name="computers">Objeto lista de computadoras</param>
        /// <param name="notes">Objeto lista de notas</param>
        /// <param name="files">Objeto lista de archivos</param>
        public Backup(List<Device> devices, List<User> users, List<Printer> printers, List<Computer> computers, List<Note> notes, List<FileObject> files, List<UserMemberOf> memberOfs)
        {
            Devices = devices;
            Users = users;
            Printers = printers;
            Computers = computers;
            Notes = notes;
            Files = files;
            MemberOfs = memberOfs;

            PrepareRelationship();
        }

        /// <summary>
        /// Crea un arreglo de bytes apartir del mismo objeto de recuperacion
        /// </summary>
        /// <returns>Arreglo de bytes</returns>
        public byte[] ToBytes()
        {
            if (this != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, this);
                    return ms.ToArray();
                }
            }
            else
            {
                throw new NullReferenceException("El objeto \"Backup\" es nulo, no se puede convertir");
            }
        }
        /// <summary>
        /// Crea un objeto de recuperacion apartir de un arreglo de bytes
        /// </summary>
        /// <param name="bytes">Arreglo de bytes</param>
        /// <returns>Objeto de recuperacion</returns>
        public static Backup FromBytes(byte[] bytes)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);

                return (Backup)formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                throw new Exception("Los datos no pueden ser leidos correctamente o no son compatibles", ex);
            }
        }

        /// <summary>
        /// Prepara los datos relacionados entre dos entidades que dependan de la existencia mutua de la misma en una lista de diccionarios
        /// </summary>
        private void PrepareRelationship()
        {
            ComputerDeviceRelationship = new List<Dictionary<string, string>>();
            
            for (int i = 0; i < Devices.Count; i++)
            {
                if (Devices[i].InUse)
                {
                    // CREA UN DICCIONARIO DEL DISPOSITIVO RELACIONADO CON EL EQUIPO DE COMPUTO
                    Dictionary<string, string> pair = new Dictionary<string, string>();
                    pair.Add(Devices[i].DeviceId, Devices[i].ParentComputerId);
                    // AGREGA EL DICCIONARIO A LA LISTA DE RELACIONES CORRESPONDIENTES
                    ComputerDeviceRelationship.Add(pair);
                    // ELIMINA DEL OBJETO DISPOSITIVO LA RELACION PARA EVITAR ERRORES DE CORRELACION AL REIMPORTAR LA INFOMRACION
                    Devices[i].InUse = false;
                    Devices[i].ParentComputerId = null;
                }
            }

            // Quita toda relacion a otras entidades para al momento de restaurar no existan problemas de correlacion
            foreach (var item in Computers)
            {
                item.Processor = null;
                item.Devices = null;
            }

            UserMemberOfRelationship = new List<Dictionary<string, string>>();

            foreach (var item in MemberOfs)
            {
                foreach (var user in item.Users)
                {
                    // CREA UN DICCIONARIO CON LA RELACION DE MEMBEROFID Y USERGUID
                    Dictionary<string, string> pair = new Dictionary<string, string>();
                    pair.Add(item.UserMemberId, user.UserGUID);
                    // AGREGA EL DICCIONARIO A LA LISTA DE RELACIONES CORRESPONDIENTES
                    UserMemberOfRelationship.Add(pair);
                }
                
                // QUITA LAS RELACIONES ACTUALES DE ESTA ENTIDAD PARA EVITAR PROBLEMAS DE CORRELACION
                item.Users = null;
            }
        }
    }
}