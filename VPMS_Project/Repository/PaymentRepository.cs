using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Data;
using VPMS_Project.Models;

namespace VPMS_Project.Repository
{
    public class PaymentRepository 
    {
        private readonly EmpStoreContext _context = null;

        public PaymentRepository(EmpStoreContext context)
        {
            _context = context;
        }
         

        public async Task<List<Payment>> GetByProjectID(int id)
        {
               return await (from a in _context.Payments.Where(x => x.ProjectId == id)
                             join b in _context.PreSalesProjects on a.ProjectId equals b.ID
                          select new Payment()
                          {
                              Id=a.Id,
                              ProjectId = a.ProjectId,
                              ProjectName = b.Title,
                              Amount = a.Amount,
                              Date = a.Date,
                              Comment=a.Comment
                          })
                     .ToListAsync();
        }

        public async Task<Payment> GetByID(int id)
        {
            return await (from a in _context.Payments.Where(x => x.Id == id)
                          join b in _context.PreSalesProjects on a.ProjectId equals b.ID
                          select new Payment()
                          {
                              Id = a.Id,
                              ProjectId = a.ProjectId,
                              ProjectName = b.Title,
                              Amount = a.Amount,
                              Date = a.Date,
                              Comment = a.Comment
                          })
                  .FirstOrDefaultAsync();
        }

        public async Task<int> AddNew(Payment model)
        {
            var newCollection = new Payment()
            {
                ProjectId = model.ProjectId,
                Amount = model.Amount,
                Comment = model.Comment,
                Date = DateTime.Now
            };
            await _context.Payments.AddAsync(newCollection);
            await _context.SaveChangesAsync();
            return newCollection.Id;
        }

        public async Task<int> Delete(int id)
        {
            var deletePayment = new Payment { Id = id };
            _context.Payments.Attach(deletePayment);
            _context.Payments.Remove(deletePayment);
            _context.SaveChanges();
            return id;
        }
    }
}
