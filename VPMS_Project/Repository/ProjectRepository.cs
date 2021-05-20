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
    public class ProjectRepository
    {
        private readonly EmpStoreContext _context = null;

        public ProjectRepository(EmpStoreContext context)
        {
            _context = context;
        }

        //new partial
        public async Task<List<ProjectModel>> GetProjects(/*int projectId*/)
        {
            return await _context.PreSalesProjects.Select(x => new ProjectModel()
            {
                ID = x.ID,
                startDate = x.startDate,
                EndDate = x.endDate,
                Description = x.Description,
                Title = x.Title,
                ProjectType = x.ProjectType,
                value = x.value,
                ProjectBudget = x.ProjectBudget,
                CustomersId = x.CustomersId,
                projectManagerId = x.projectManagerId

            }).ToListAsync();
        }
        //finish


        public async Task<int> AddNewProject(ProjectModel model)
        {
            var newProject = new PreSalesProjects()
            {
                startDate = model.startDate,
                Description = model.Description,
                Title = model.Title,
                endDate = model.EndDate,
                ProjectType = model.ProjectType,
                value = model.value,
                ProjectBudget = model.ProjectBudget,
                CustomersId = model.CustomersId,
                projectManagerId = model.projectManagerId,
                preProjectState = true

            };

            await _context.PreSalesProjects.AddAsync(newProject);
            await _context.SaveChangesAsync();

            return newProject.ID;
        }

        //new part
        public async Task<List<ProjectModel>> SearchProject(string title)
        {
            var projects = new List<ProjectModel>();
            var allprojects = await _context.PreSalesProjects.ToListAsync();
            if (allprojects?.Any() == true)
            {
                foreach (var project in allprojects)
                {
                    if (project.Title.Contains(title))
                    {
                        projects.Add(new ProjectModel()
                        {
                            ID = project.ID,
                            Title = project.Title,
                            Description = project.Description,
                            ProjectType = project.ProjectType,
                        });
                    }
                }
            }
            return projects;
        }
        //finish part

        public async Task<List<ProjectModel>> GetAllProjects()
        {
            var projects = new List<ProjectModel>();

            var allprojects = await _context.PreSalesProjects.Where(e => e.preProjectState == true).ToListAsync();
            if (allprojects?.Any() == true)
            {
                foreach (var project in allprojects)
                {
                    projects.Add(new ProjectModel()
                    {
                        ID = project.ID,
                        Title = project.Title,
                        Description = project.Description,
                        ProjectType = project.ProjectType,
                        projectManagerId = project.projectManagerId,

                        startDate = project.startDate,
                        EndDate = project.startDate,
                        value = project.value,
                        ProjectBudget = project.ProjectBudget,
                        CustomersId = project.CustomersId,
                    });
                }
            }
            return projects;
        }

        public async Task<ProjectModel> GetProjectByID(int id)
        {
            return await _context.PreSalesProjects.Where(x => x.ID == id)
                .Select(project => new ProjectModel()
                {
                    Description = project.Description,
                    ID = project.ID,
                    Title = project.Title,
                    ProjectType = project.ProjectType,
                    projectManagerId = project.projectManagerId,
                    projectManager = project.PreSalesprojectManager.Name,
                    startDate = project.startDate,
                    EndDate = project.endDate,
                    value = project.value,
                    ProjectBudget = project.ProjectBudget,
                    CustomersId = project.CustomersId,
                    Customers = project.Customers.Name,

                }).FirstOrDefaultAsync();

            //return await (from p in _context.PreSalesProjects.Where(x => (x.ID == id))
            //              join c in _context.PreSalesCustomers on p.CustomersId equals c.Id
            //              join pm in _context.PreSalesProjectManagers on p.projectManagerId equals pm.Id
            //              select new ProjectModel()
            //              {
            //                  Description = p.Description,
            //                  ID = p.ID,
            //                  Title = p.Title,
            //                  ProjectType = p.ProjectType,
            //                  projectManagerId = p.projectManagerId,
            //                  projectManager = pm.Name,
            //                  startDate = p.startDate,
            //                  EndDate = p.endDate,
            //                  value = p.value,
            //                  ProjectBudget = p.ProjectBudget,
            //                  CustomersId = p.CustomersId,
            //                  Customers = c.name
            //              }).FirstOrDefaultAsync();
        }

        // new part
        public async Task<bool> EditProjects(ProjectModel project)
        {
            var pro = await _context.PreSalesProjects.FindAsync(project.ID);

            pro.Title = project.Title;
            pro.Description = project.Description;
            pro.ProjectType = project.ProjectType;
            pro.startDate = project.startDate;
            pro.endDate = project.EndDate;
            pro.value = project.value;
            pro.ProjectBudget = project.ProjectBudget;

            project.projectManager = project.projectManager;
            //pro.Customers = project.Customers;

            _context.Entry(pro).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
        //new part finish

        //delete part
        public async Task<bool> DeleteProject(int id)
        {
            var projects = await _context.PreSalesProjects.FindAsync(id);

            _context.PreSalesProjects.Remove(projects);
            await _context.SaveChangesAsync();
            return true;

        }

        //delete the project after going to add new project
        public async Task<bool> AddToDeletedProjects(int id)
        {
            var projects = await _context.PreSalesProjects.FindAsync(id);

            var NewProject = new PreSalesDeletedProjects
            {
                Title = projects.Title,
                Description = projects.Description,
                ProjectType = projects.ProjectType,
                value = projects.value,
                ProjectBudget = projects.ProjectBudget,
                CustomersId = projects.CustomersId,
                projectManagerId = projects.projectManagerId,
                startDate = projects.startDate,
                endDate = projects.endDate,

            };
            await _context.PreSalesDeletedProjects.AddAsync(NewProject);
            await _context.SaveChangesAsync();

            return true;
        }

        //Finish delete part

        //public List<ProjectModel> SearchProject(string title, string projectClientId)
        //{
        //    return null;
        //}


        //convert part
        public async Task<bool> ConvertProject(int id)
        {
            var convertProjects = await _context.PreSalesProjects.FindAsync(id);

            convertProjects.preProjectState = false;

            _context.PreSalesProjects.Update(convertProjects);
            await _context.SaveChangesAsync();
            return true;

        }


        //convert the project after going to add new project
        public async Task<bool> AddToConvertProjects(int id)
        {
            var projects = await _context.PreSalesProjects.FindAsync(id);

            var NewConvertProject = new Projects
            {
                Name = projects.Title,
                Description = projects.Description,
                Type = projects.ProjectType,
                EstimetedBudget = projects.value,
                ContractValue = projects.ProjectBudget,
                CustomersId = projects.CustomersId,
                EmployeesId = projects.projectManagerId,
                StartDate = projects.startDate,
                ClosedDate = projects.endDate,
                DeliveryDate = projects.endDate,
                CreatedDate = projects.startDate,
                LastUpdate = projects.startDate,
                AllocatedTasks = 0,
                FinalizedTasks = 0
            };
            await _context.Projects.AddAsync(NewConvertProject);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

