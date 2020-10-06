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
        public List<Computer> Computers { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Note> Notes { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<FileObject> Files { get; set; }
        // ENTIDAD SECUNDARIA DEPENDIENTE
        public List<Dictionary<string, string>> ComputerDeviceRelationship { get; set; }

        public Backup()
        {
        }
        public Backup(List<Device> devices, List<User> users, List<Computer> computers, List<Note> notes, List<FileObject> files)
        {
            Devices = devices;
            Users = users;
            Computers = computers;
            Notes = notes;
            Files = files;

            PrepareRelationship();
        }

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
        public Backup FromBytes(byte[] bytes)
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
        }
    }
}