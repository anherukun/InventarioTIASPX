using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryGenericFiles : RepositoryFiles
    {
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void Add(FileObject fileObject)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override List<FileObject> GetAll(string parentId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override bool HasFilesRelated(string parentId)
        {
            throw new NotImplementedException();
        }

        public override void BreakRelationship(string fileId)
        {
            FileObject file = new RepositoryGenericFiles().Get(fileId, true);
            file.ParentObjectId = null;

            using (var db = new InventoryTIASPXContext())
            {
                db.Files.AddOrUpdate(file);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET Computer_ComputerId = NULL WHERE fileId LIKE \"{file.FileId}\"");
                db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET User_UserGUID = NULL WHERE fileId LIKE \"{file.FileId}\"");
            }
        }
        public override void Delete(string fileId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(fileId, true)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public override FileObject Get(string fileId, bool includeData)
        {
            using (var db = new InventoryTIASPXContext())
            {
                if (includeData)
                    return db.Files.Where(x => x.FileId == fileId).FirstOrDefault();
                else
                {
                    var o = db.Files.Where(x => x.FileId == fileId).Select(x => new
                    {
                        FileId = x.FileId,
                        Name = x.Name,
                        Mime = x.Mime,
                        Size = x.Size,
                        UploadedTicks = x.UploadedTicks,
                        ParentObjectId = x.ParentObjectId
                    }).FirstOrDefault();

                    return new FileObject()
                    {
                        FileId = o.FileId,
                        Name = o.Name,
                        Mime = o.Mime,
                        Size = o.Size,
                        UploadedTicks = o.UploadedTicks,
                        ParentObjectId = o.ParentObjectId
                    };
                }
            }
        }
        public override List<FileObject> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                List<FileObject> list = new List<FileObject>();

                var o = db.Files.Select(x => new {
                    FileId = x.FileId,
                    Name = x.Name,
                    Mime = x.Mime,
                    Size = x.Size,
                    UploadedTicks = x.UploadedTicks,
                    ParentObjectId = x.ParentObjectId
                }).OrderBy(x => x.UploadedTicks).ToList();

                foreach (var item in o)
                {
                    list.Add(new FileObject
                    {
                        FileId = item.FileId,
                        Name = item.Name,
                        Mime = item.Mime,
                        Size = item.Size,
                        UploadedTicks = item.UploadedTicks,
                        ParentObjectId = item.ParentObjectId
                    });
                }

                return list;
            }
        }
        public override List<FileObject> GetAllWithData()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Files.ToList();
            }
        }
    }
}