using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryPrinterFiles : RepositoryFiles
    {
        // NO IMPLEMENTADO EN ESTA CLASE
        public override List<FileObject> GetAll()
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override List<FileObject> GetAllWithData()
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void BreakRelationship(string noteId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void Delete(string fileId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override FileObject Get(string fileId, bool includeData)
        {
            throw new NotImplementedException();
        }

        public override void Add(FileObject fileObject)
        {
            string printerId = fileObject.ParentObjectId;
            fileObject.ParentObjectId = $"PRT_{printerId}";

            using (var db = new InventoryTIASPXContext())
            {
                db.Files.Add(fileObject);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET Printer_PrinterId = \"{printerId}\" WHERE fileId LIKE \"{fileObject.FileId}\"");
            }
        }
        public override List<FileObject> GetAll(string parentId)
        {
            string parentObjectId = $"PRT_{parentId}";
            using (var db = new InventoryTIASPXContext())
            {
                List<FileObject> list = new List<FileObject>();

                var o = db.Files.Where(x => x.ParentObjectId == parentObjectId).Select(x => new {
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
        public override bool HasFilesRelated(string parentId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                string parentFixed = $"PRT_{parentId}";
                return db.Files.Any(x => x.ParentObjectId == parentFixed);
            }
        }
    }
}