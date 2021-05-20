using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Data;
using VPMS_Project.Models;

namespace VPMS_Project.Repository
{
    public class BudgetRepository
    {
        private readonly EmpStoreContext _context = null;

        public BudgetRepository(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetModel>> GetBudgets()
        {
            return await _context.PreSalesBudget.Select(x => new BudgetModel()
            {
                Id = x.Id,
                Actual = x.Actual,
                LbHou = x.LbHou,
                LbRate = x.LbRate,
                MtUnit = x.MtUnit,
                MtunitPr = x.MtunitPr,
                Under = x.Under,
                cost = x.cost,
                BudgetCost = x.BudgetCost,
                ProjectsID = x.ProjectsID,

                projectName = x.PreSalesProjects.Title


            }).ToListAsync();
        }

        public async Task<int> AddNewBudget(BudgetModel model)
        {
            var newBudget = new PreSalesBudget()
            {
                Id = model.Id,
                Actual = model.Actual,
                LbHou = model.LbHou,
                LbRate = model.LbRate,
                MtUnit = model.MtUnit,
                MtunitPr = model.MtunitPr,
                Under = model.Under,
                cost = model.cost,
                BudgetCost = model.BudgetCost,
                ProjectsID = model.ProjectsID


            };
            await _context.PreSalesBudget.AddAsync(newBudget);
            await _context.SaveChangesAsync();

            return newBudget.Id;
        }

        internal List<ChartType> GetChartByProjectId(int projectId)
        {
            
                List<BudgetLineItem> data = (from t in _context.VW_BudgetLineItem.Where(x => x.ProjectId == projectId)
                                           select new BudgetLineItem()
                                           {
                                               TaskID = t.TaskID,                                               
                                               EstimatedCost = t.EstimatedCost,
                                               ActualCost = t.ActualCost                                               
                                           }).ToList();

            List<ChartType> tsk = new List<ChartType>();            
                foreach (var dat in data)
                {
                tsk.Add(new ChartType { X = "Task ID: "+dat.TaskID.ToString(), Y = dat.EstimatedCost.ToString() });
                tsk.Add(new ChartType { X = "", Y = dat.ActualCost.ToString() });               
                tsk.Add(new ChartType { X = "", Y = "0" });
            }
                return tsk;           
        }

        internal List<ChartType> GetRevenues()
        {

            List<ProjectSummary> data = (from t in _context.VW_ProjectSummary.ToList()
                                         select new ProjectSummary()
                                         {
                                             ProjectName = t.ProjectName,
                                             Settled = t.Settled                                             
                                         }).ToList();

            List<ChartType> tsk = new List<ChartType>();
            foreach (var dat in data)
            {
                tsk.Add(new ChartType { X =dat.ProjectName, Y = dat.Settled.ToString() });               
            }
            return tsk;
        }

        internal List<ChartType> GetSixMonthRevenues()
        {
            DateTime firstDate =new DateTime (DateTime.Now.AddMonths(-7).Year, DateTime.Now.AddMonths(-7).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-7).Year, DateTime.Now.AddMonths(-7).Month));
            List<ChartType> sixMonthPayments = (from p in _context.Payments.Where(x=>x.Date>firstDate)                                             
                                         select new ChartType()
                                         {
                                             X = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Date.Month),
                                             Y= p.Amount.ToString()
                                         }).ToList();

            List<ChartType> tsk = sixMonthPayments
            .GroupBy(l => l.X)
            .Select(cl => new ChartType
            {
                X = cl.First().X,
                Y = cl.Sum(c => Convert.ToDouble(c.Y)).ToString()
            }).ToList();

            return tsk;
        }

        internal List<ChartType> GetbudgetsforDsboard()
        {

            List<ProjectSummary> data = (from t in _context.VW_ProjectSummary.ToList()
                                         select new ProjectSummary()
                                         {
                                             ProjectName = t.ProjectName,                                             
                                             CurrentActualCost=t.CurrentActualCost,
                                             CurrentEstimatedCost=t.CurrentEstimatedCost,
                                             Settled = t.Settled
                                         }).ToList();

            List<ChartType> tsk = new List<ChartType>();
            foreach (var dat in data)
            {
                tsk.Add(new ChartType { X = dat.ProjectName, Y = dat.CurrentActualCost.ToString() });
                tsk.Add(new ChartType { X =  "", Y = dat.CurrentEstimatedCost.ToString() });
                tsk.Add(new ChartType { X = "", Y = "0" });
            }
            return tsk;
        }

        public async Task<List<BudgetModel>> GetAllBudgets()
        {
            var budgets = new List<BudgetModel>();
            var allbudgets = await _context.PreSalesBudget.ToListAsync();
            if (allbudgets?.Any() == true)
            {
                foreach (var budget in allbudgets)
                {
                    budgets.Add(new BudgetModel()
                    {
                        Id = budget.Id,
                        Actual = budget.Actual,
                        LbHou = budget.LbHou,
                        LbRate = budget.LbRate,
                        MtUnit = budget.MtUnit,
                        MtunitPr = budget.MtunitPr,
                        Under = budget.Under,
                        cost = budget.cost,
                        BudgetCost = budget.BudgetCost,
                        ProjectsID = budget.ProjectsID,

                        //projectName = budget.Projects.Title                       

                    });
                }
            }
            return budgets;
        }
        
        public  List<ProjectSummary> ProjectsSummery()
        {
            return (from t in _context.VW_ProjectSummary.ToList()
                  select new ProjectSummary()
                  {
                      Id = t.Id,
                      Budget = t.Budget,
                      CurrentActualCost = t.CurrentActualCost,
                      CurrentEstimatedCost=t.CurrentEstimatedCost,
                      ProjectName=t.ProjectName,
                      Settled=t.Settled,
                      EstimatedCost=t.EstimatedCost
                  }).ToList();
        }

        //new part

        //public async Task<bool> TotalCost(BudgetModel budgetModel)
        //{
        //    var bgt = await _context.Budget.FindAsync(budgetModel.ProjectsID);

        //    bgt.BudgetCost = bgt.LbHou * bgt.LbRate + bgt.MtUnit * bgt.MtunitPr;

        //    _context.Entry(bgt).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return true;
        //}


        //finish

        public async Task<List<BudgetLineItem>> GetBudgetByProjectId(int projectId)
        {
            return await (from t in _context.VW_BudgetLineItem.Where(x => x.ProjectId == projectId)

                          select new BudgetLineItem()
                          {
                              TaskID = t.TaskID,
                              TaskName = t.TaskName,
                              Employee = t.Employee,
                              Time = t.Time,
                              EstimatedCost = t.EstimatedCost,
                              ActualCost = t.ActualCost,
                              IsCompleted = t.IsCompleted,
                              Other = 0
                          })
          .ToListAsync();

            //  return await (from t in _context.Tasks.Where(x => x.ProjectsId == projectId)
            //              join emp in _context.Employees on t.EmployeesId equals emp.EmpId
            //              join s in _context.Salarys on emp.Designation equals s.Designation
            //              join tt in _context.TimeSheetTask on t.Id equals tt.TaskId
            //              select new BudgetLineItem()
            //              {                             
            //                  TaskID=t.Id,
            //                  TaskName=t.Name,
            //                  Employee=t.EmployeeName,
            //                  Time=t.AllocatedHours,
            //                  EstimatedCost=s.HourlyRate * t.AllocatedHours,
            //                  ActualCost= s.HourlyRate * tt.TotalHours,//todo calculate actual cost  
            //                  IsCompleted = (bool)t.TaskComplete,
            //                  Other=0
            //              })
            //.ToListAsync();     


            //var budgets = new List<BudgetModel>();
            //var allbudgets = await _context.PreSalesBudget.ToListAsync();
            //if (allbudgets?.Any() == true)
            //{
            //    foreach (var budget in allbudgets)
            //    {
            //        if (budget.ProjectsID == projectId)
            //        {
            //            budgets.Add(new BudgetModel()
            //            {
            //                Id = budget.Id,
            //                Actual = budget.Actual,
            //                LbHou = budget.LbHou,
            //                LbRate = budget.LbRate,
            //                MtUnit = budget.MtUnit,
            //                MtunitPr = budget.MtunitPr,
            //                Under = budget.Under,
            //                cost = budget.cost,
            //                BudgetCost = budget.BudgetCost,
            //                ProjectsID = budget.ProjectsID,

            //                //projectName = budget.Projects.Title                       

            //            });
            //        }

            //    }
            //}
            //return budgets;
        }

        //Finish

   public bool doesNeedReminder(int projectId)
        {
            List<double> preSellCollections = _context.PreSalescollection.Where(x => x.Id == projectId).OrderBy(x => x.Id).Select(x => x.value).ToList();
            double totalActualcost = _context.VW_BudgetLineItem.Where(x => x.ProjectId == projectId).Where(x=>x.IsCompleted).Select(x => x.ActualCost).Sum();
            double totalPaidAmount = _context.Payments.Where(x => x.ProjectId == projectId).Select(x => x.Amount).Sum();
            double amountToSettled = totalActualcost - totalPaidAmount;
            double cumulativeCollection = 0;
            if (amountToSettled < 0)
            {
                return false;                        
            }

            foreach(double thisCollection in preSellCollections)
            {               
               cumulativeCollection += thisCollection;
                if ((cumulativeCollection < totalActualcost) && ((cumulativeCollection + thisCollection) > totalActualcost)) 
                {
                    if (amountToSettled > (thisCollection/4)) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<BudgetModel> GetBudgetById(int id)
        {
            return await _context.PreSalesBudget.Where(x => x.Id == id)
                .Select(budget => new BudgetModel()
                {
                    Actual = budget.Actual,
                    LbHou = budget.LbHou,
                    LbRate = budget.LbRate,
                    MtUnit = budget.MtUnit,
                    MtunitPr = budget.MtunitPr,
                    Under = budget.Under,
                    cost = budget.cost,
                    BudgetCost = budget.BudgetCost,
                    ProjectsID = budget.ProjectsID,
                    projectName = budget.PreSalesProjects.Title

                }).FirstOrDefaultAsync();
        }

        public async Task<bool> EditBudgets(BudgetModel budget)
        {
            var bug = await _context.PreSalesBudget.FindAsync(budget.Id);

            bug.Actual = budget.Actual;
            bug.LbHou = budget.LbHou;
            bug.LbRate = budget.LbRate;
            bug.MtUnit = budget.MtUnit;
            bug.MtunitPr = budget.MtunitPr;
            bug.Under = budget.Under;
            bug.cost = budget.cost;
            bug.BudgetCost = budget.BudgetCost;
            bug.ProjectsID = budget.ProjectsID;


            _context.Entry(bug).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }

        //delete part
        public async Task<bool> DeleteBudget(int id)
        {
            var budgets = await _context.PreSalesBudget.FindAsync(id);

            _context.PreSalesBudget.Remove(budgets);
            await _context.SaveChangesAsync();
            return true;

        }

        //delete the customer after going to add new customer
        public async Task<bool> AddToDeletedBudgets(int id)
        {
            var budget = await _context.PreSalesBudget.FindAsync(id);

            var NewBudget = new PreSalesDeleteBudgets
            {

                Actual = budget.Actual,
                LbHou = budget.LbHou,
                LbRate = budget.LbRate,
                MtUnit = budget.MtUnit,
                MtunitPr = budget.MtunitPr,
                Under = budget.Under,
                cost = budget.cost,
                BudgetCost = budget.BudgetCost
            };
            await _context.PreSalesDeleteBudgets.AddAsync(NewBudget);
            await _context.SaveChangesAsync();

            return true;
        }

        //Finish delete part


        //public List<BudgetModel> SearchBudgets (int id, string name)
        //{
        //    return DataSource().Where(x => x.ProjectName.Contains(name)).ToList();
        //}




    }
}
