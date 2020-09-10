﻿using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services.Abstracts
{
    public abstract class RepositoryNotes
    {
        /// <summary>
        /// Agrega un nuevo <see cref="Note"/> al registro
        /// </summary>
        /// <param name="note">Objeto <see cref="Note"/></param>
        public abstract void Add(Note note);
        /// <summary>
        /// Obtiene un registro con el <paramref name="noteId"/> y los regresa en un <see cref="Note"/>/>
        /// </summary>
        /// <param name="noteId">Propiedad identificadora del objeto <see cref="Note"/></param>
        /// <returns>Lista de <see cref="Note"/></returns>
        public abstract Note Get(string noteId);
        /// <summary>
        /// Obtiene todos los registros y los regresa en una lista de <see cref="Note"/>/>
        /// </summary>
        /// <returns>Lista de <see cref="FileObject"/></returns>
        public abstract List<Note> GetAll();
        /// <summary>
        /// Obtiene todos los registros relacionados con el <paramref name="parentId"/> y los regresa en una lista de <see cref="Note"/>/>
        /// </summary>
        /// <param name="parentId">Propiedad que relaciona el registro con el objeto padre</param>
        /// <returns>Lista de <see cref="Note"/></returns>
        public abstract List<Note> GetAll(string parentId);
        /// <summary>
        /// Elimina un <see cref="Note"/> del registro
        /// </summary>
        /// <param name="noteId">Propiedad identificadora del objeto <see cref="Note"/></param>
        public abstract void Delete(string noteId);
    }
}