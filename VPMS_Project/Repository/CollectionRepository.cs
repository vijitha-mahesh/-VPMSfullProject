using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using VPMS_Project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Repository
{
    public class CollectionRepository
    {
        private readonly EmpStoreContext _context = null;

        public CollectionRepository(EmpStoreContext context)
        {
            _context = context;
        }


        public async Task<int> AddNewCollection(CollectionModel model)
        {
            var newCollection = new PreSalescollection()
            {
                Description = model.Description,
                value = model.value,
                ProjecstID = model.ProjectsID,

            };

            await _context.PreSalescollection.AddAsync(newCollection);
            await _context.SaveChangesAsync();

            return newCollection.Id;
        }

        public async Task<int> Delete(int id)
        {
            var deleteCollection = new PreSalescollection { Id = id };
            _context.PreSalescollection.Attach(deleteCollection);
            _context.PreSalescollection.Remove(deleteCollection);
            _context.SaveChanges();
            return id;
        }

        //public async Task<List<CollectionModel>> GetAllCollection(int entries)
        //{
        //    var collections = new List<CollectionModel>();
        //    var allcollections = await _context.collection.ToListAsync();
        //    if (allcollections?.Any() == true)
        //    {
        //        foreach (var collection in allcollections)
        //        {
        //            collections.Add(new CollectionModel()
        //            {
        //                Id = collection.Id,
        //                Description = collection.Description,
        //                value = collection.value,
        //                ProjectsID = collection.ProjecstID

        //            });
        //        }
        //    }
        //    return collections;
        //}


        public async Task<List<CollectionModel>> GetAllCollection(int projectId)
        {
            var collections = new List<CollectionModel>();
            //var allcollections = await _context.PreSalescollection.ToListAsync();
            //if (allcollections?.Any() == true)
            //{
            //    foreach (var collection in allcollections)
            //    {
            //        if (collection.ProjecstID == projectId)
            //        {
            //            collections.Add(new CollectionModel()
            //            {
            //                Id = collection.Id,
            //                Description = collection.Description,
            //                value = collection.value,
            //                ProjectsID = collection.ProjecstID,
            //                Projects = collection.PreSalesProjects.Title,

            //            });
            //        }

            //    }
            //}

            collections = await (from c in _context.PreSalescollection.Where(x => x.ProjecstID == projectId)
                                 select new CollectionModel()
                                 {
                                     Id = c.Id,
                                     Description = c.Description,
                                     value = (int)c.value,
                                     ProjectsID = c.ProjecstID
                                 }).ToListAsync();
            return collections;


            //return await (from a in _context.Employees.Where(x => (x.Status == "Active"))
            //              join b in _context.Job on a.JobTitleId equals b.JobId
            //              select new EmpModel()
            //              {
            //                  EmpId = a.EmpId,
            //                  EmpFullName = a.EmpFName + " " + a.EmpLName
            //              })
            //              .ToListAsync();
        }

        public async Task<CollectionModel> GetCollectionById(int Id)
        {

            return await (from c in _context.PreSalescollection.Where(x => x.Id == Id)
                          select new CollectionModel()
                          {
                              Id = c.Id,
                              Description = c.Description,
                              value = (int)c.value,
                              ProjectsID = c.ProjecstID
                          }).FirstOrDefaultAsync();
        }



    }
}
