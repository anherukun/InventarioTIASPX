using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services.Abstracts
{
    public abstract class RepositoryFiles
    {
        /// <summary>
        /// Agrega un nuevo <see cref="FileObject"/> al registro
        /// </summary>
        /// <param name="fileObject">Objeto <see cref="FileObject"/></param>
        public abstract void Add(FileObject fileObject);
        /// <summary>
        /// Obtiene un registro con el <paramref name="fileId"/> y los regresa en un <see cref="FileObject"/>. Obteniendo el valor de <see cref="FileObject.Data"/>
        /// </summary>
        /// <param name="fileId">Propiedad identificadora del objeto <see cref="FileObject"/></param>
        /// <param name="includeData">Propiedad que determina si incluir la informacion adjunta o no</param>
        /// <returns>Lista de <see cref="FileObject"/></returns>
        public abstract FileObject Get(string fileId, bool includeData);

        /// <summary>
        /// Obtiene todos los registros y los regresa en una lista de <see cref="FileObject"/>. Pero sin obtener el valor de <see cref="FileObject.Data"/>
        /// </summary>
        /// <returns>Lista de <see cref="FileObject"/></returns>
        public abstract List<FileObject> GetAll();
        /// <summary>
        /// Obtiene todos los registros relacionados con el <paramref name="parentId"/> y los regresa en una lista de <see cref="FileObject"/>. Pero sin obtener el valor de <see cref="FileObject.Data"/>
        /// </summary>
        /// <param name="parentId">Propiedad que relaciona el registro con el objeto padre</param>
        /// <returns>Lista de <see cref="FileObject"/></returns>
        public abstract List<FileObject> GetAll(string parentId);
        //public abstract void AssignParent(string fileId, string parentId);
        //public abstract void UnassignParent(string fileId);
        /// <summary>
        /// Elimina un <see cref="FileObject"/> del registro
        /// </summary>
        /// <param name="fileId">Propiedad identificadora del objeto <see cref="FileObject"/></param>
        public abstract void Delete(string fileId);
    }
}