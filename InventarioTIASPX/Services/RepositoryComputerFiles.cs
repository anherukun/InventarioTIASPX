﻿using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryComputerFiles : RepositoryFiles
    {
        public override void Add(FileObject fileObject)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.ComputersFiles.Add(fileObject);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET Computer_ComputerId = \"{fileObject.ParentObjectId}\" WHERE fileId LIKE \"{fileObject.FileId}\"");
            }
        }
        public override FileObject Get(string fileId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.ComputersFiles.Where(x => x.FileId == fileId).FirstOrDefault();
            }
        }

        public override List<FileObject> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                List<FileObject> list = new List<FileObject>();

                var o = db.ComputersFiles.Select(x => new {
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
        public override List<FileObject> GetAll(string parentId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                List<FileObject> list = new List<FileObject>();

                var o = db.ComputersFiles.Where(x => x.ParentObjectId == parentId).Select(x => new {
                    FileId = x.FileId,
                    Name = x.Name,
                    Mime = x.Mime,
                    Size = x.Size,
                    UploadedTicks = x.UploadedTicks,
                    ParentObjectId = x.ParentObjectId
                }).OrderBy(x => x.UploadedTicks).ToList();

                foreach (var item in o)
                {
                    list.Add(new FileObject {
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

        // public override void AssignParent(string fileId, string parentId)
        // {
        //     FileObject file = new RepositoryComputerFiles().Get(fileId);
        //     file.ParentObjectId = parentId;
        //     using (var db = new InventoryTIASPXContext())
        //     {
        //         db.ComputersFiles.AddOrUpdate(file);
        //         db.SaveChanges();
        // 
        //         db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET Computer_ComputerId = \"{parentId}\" WHERE fileId LIKE \"{fileId}\"");
        //     }
        // }
        // public override void UnassignParent(string fileId)
        // {
        //     FileObject file = new RepositoryComputerFiles().Get(fileId);
        //     file.ParentObjectId = null;
        //     using (var db = new InventoryTIASPXContext())
        //     {
        //         db.ComputersFiles.AddOrUpdate(file);
        //         db.SaveChanges();
        // 
        //         db.Database.ExecuteSqlCommand($"UPDATE fileobjects SET Computer_ComputerId = null WHERE fileId LIKE \"{fileId}\"");
        //     }
        // }

        public override void Delete(string fileId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(fileId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}