using Microsoft.EntityFrameworkCore;
using VPMS_Project.Data;
using VPMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPMS_Project.Repository
{
    public class TaskRepository
    {
        private readonly EmpStoreContext _context = null;

        public TaskRepository(EmpStoreContext context)
        {
            _context = context;
        }

        //new part
        public async Task<List<PreTaskModel>> GetTasks()
        {
            return await _context.PreSalesTasks.Select(x => new PreTaskModel()
            {
                Id = x.Id,
                StartDate = x.StartDate,
                Number = x.Number,
                Description = x.Description,
                EndDate = x.EndDate,
                Priority = x.Priority,
                Assignee = x.Assignee,
                ProjectsID = x.ProjectsID,


            }).ToListAsync();
        }

        //Finish


        public async Task<List<PreTaskModel>> GetTaskByProjectId(int projectId, string Title)
        {
            var tasks = new List<PreTaskModel>();
            var alltasks = await _context.PreSalesTasks.ToListAsync();
            if (alltasks?.Any() == true)
            {
                foreach (var task in alltasks)
                {
                    if (task.ProjectsID == projectId)
                    {
                        tasks.Add(new PreTaskModel()
                        {
                            Id = task.Id,
                            Number = task.Number,
                            Description = task.Description,
                            Duration = task.Duration,
                            StartDate = task.StartDate,
                            EndDate = task.EndDate,
                            Priority = task.Priority,
                            Assignee = task.Assignee,
                            ProjectsID = task.ProjectsID
                        });
                    }

                }
            }

            return tasks;
        }


        public async Task<int> AddNewTask(PreTaskModel model)
        {
            var newTask = new PreSalesTasks()
            {
                Number = model.Number,
                Description = model.Description,
                Duration = model.Duration,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Priority = model.Priority,
                Assignee = model.Assignee,
                ProjectsID = model.ProjectsID,
                IsAssigned = false
    };

            await _context.PreSalesTasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            return newTask.Id;
        }

        public async Task<List<PreTaskModel>> GetAllTasks()
        {
            var tasks = new List<PreTaskModel>();
            var alltasks = await _context.PreSalesTasks.ToListAsync();
            if (alltasks?.Any() == true)
            {
                foreach (var task in alltasks)
                {
                    tasks.Add(new PreTaskModel()
                    {
                        Id = task.Id,
                        Number = task.Number,
                        Description = task.Description,
                        Duration = task.Duration,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        Priority = task.Priority,
                        Assignee = task.Assignee,
                        ProjectsID = task.ProjectsID
                    });

                }
            }

            return tasks;
        }

        public async Task<PreTaskModel> GetTaskById(int id)
        {
            return await _context.PreSalesTasks.Where(x => x.Id == id)
                .Select(task => new PreTaskModel()
                {
                    Number = task.Number,
                    Description = task.Description,
                    Duration = task.Duration,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    Assignee = task.Assignee,
                    ProjectsID = task.ProjectsID,
                    projectName = task.PreSalesProjects.Title
                }).FirstOrDefaultAsync();

        }

        //edit task part
        public async Task<bool> EditTasks(PreTaskModel task)
        {
            var tas = await _context.PreSalesTasks.FindAsync(task.Id);

            tas.Description = task.Description;
            tas.Number = task.Number;
            tas.Duration = task.Duration;
            tas.StartDate = task.StartDate;
            tas.EndDate = task.EndDate;
            tas.Priority = task.Priority;
            tas.Assignee = task.Assignee;
            tas.ProjectsID = task.ProjectsID;

            _context.Entry(tas).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }
        //Finish edit task part


        //delete part
        public async Task<bool> DeleteTask(int id)
        {
            var tasks = await _context.PreSalesTasks.FindAsync(id);

            _context.PreSalesTasks.Remove(tasks);
            await _context.SaveChangesAsync();
            return true;

        }

        //delete the customer after going to add new task
        public async Task<bool> AddToDeletedTasks(int id)
        {
            var tasks = await _context.PreSalesTasks.FindAsync(id);

            var NewTask = new PreSalesDeletedTasks
            {
                Number = tasks.Number,
                Description = tasks.Description,
                Duration = tasks.Duration,
                StartDate = tasks.StartDate,
                EndDate = tasks.EndDate,
                Priority = tasks.Priority,
                Assignee = tasks.Assignee,

            };
            await _context.PreSalesDeletedTasks.AddAsync(NewTask);
            await _context.SaveChangesAsync();

            return true;
        }
        //Finish delete part





    }
}
