using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using VPMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Document = VPMS_Project.Data.PreSalesDocument;

namespace VPMS_Project.Repository
{
    public class DocumentRepository
    {
        private readonly EmpStoreContext _context = null;
        

        public DocumentRepository(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<DocumentModel>> GetDocument()
        {
            return await _context.PreSalesDocument.Select(x => new DocumentModel()
            {
                ID = x.ID,
                ScopeDocumentUrl = x.ScopeDocumentUrl,
                ActionPlanUrl = x.ActionPlanUrl,
                TimePlanUrl = x.TimePlanUrl,
                ProjectsID = x.ProjectsID

            }).ToListAsync();
        }



        public async Task<int> AddNewDocument(DocumentModel model)
        {
            var newDocument = new Document()
            {

                ScopeDocumentUrl = model.ScopeDocumentUrl,
                ActionPlanUrl = model.ActionPlanUrl,
                TimePlanUrl = model.TimePlanUrl,
                ProjectsID = model.ProjectsID

            };

            await _context.PreSalesDocument.AddAsync(newDocument);
            await _context.SaveChangesAsync();

            return newDocument.ID;
        }

        //public async Task<DocumentModel> GetDocumentByID(int id)
        //{
        //    return await _context.Document.Where(x => x.ID == id)
        //        .Select(document => new DocumentModel()
        //    {
        //        ScopeDocumentUrl = document.ScopeDocumentUrl,
        //        ActionPlanUrl = document.ActionPlanUrl,
        //        TimePlanUrl = document.TimePlanUrl,
        //        ProjectsID = document.ProjectsID

        //    }).FirstOrDefaultAsync();
        //}



        public async Task<List<DocumentModel>> GetDocument(int projectId)
        {
            var documents = new List<DocumentModel>();
            var alldocuments = await _context.PreSalesDocument.ToListAsync();
            if (alldocuments?.Any() == true)
            {
                foreach (var document in alldocuments)
                {
                    if (document.ProjectsID == projectId)
                    {
                        documents.Add(new DocumentModel()
                        {
                            ID = document.ID,
                            ScopeDocumentUrl = document.ScopeDocumentUrl,
                            ActionPlanUrl = document.ActionPlanUrl,
                            TimePlanUrl = document.TimePlanUrl,
                            ProjectsID = document.ProjectsID
                        });
                    }

                }
            }
            return documents;
        }
    }
}
