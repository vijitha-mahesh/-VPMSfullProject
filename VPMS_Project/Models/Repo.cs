using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Data;

namespace VPMS_Project.Models
{
    public class Repo
    {

        private readonly EmpStoreContext _context = null;

        public Repo(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjects()
        {
            var projects = new List<Project>();
            var AllProjects = await _context.Projects.OrderByDescending(x => x.Id).ToListAsync();
            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    projects.Add(new Project()
                    {
                        Id = project.Id,
                        Description = project.Description,
                        Name = project.Name,
                        Status = project.Status
                    });
                }
            }
            return projects;
        }

        public async Task<List<Project2>> GetProjects2()
        {
            var projects = new List<Project2>();
            var AllProjects = await _context.Projects.OrderByDescending(x => x.Id).ToListAsync();
            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    var pm = await _context.Employees.FindAsync(project.EmployeesId);

                    var cus = "none";
                    if (project.CustomersId != null)
                    {
                        var customer = await _context.Customers.FindAsync(project.CustomersId);
                        cus = customer.Name;
                    }



                    projects.Add(new Project2()
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Type = project.Type,
                        Status = project.Status,
                        ProjectManagerName = pm.EmpFName,
                        Cost = project.Cost,
                        Budget = project.EstimetedBudget,
                        ContractValue = (project.ContractValue == null) ? 0 : project.ContractValue,
                        CustomerName = cus,
                        Completion = 0,
                        CurrentProfit = (project.ContractValue == null) ? 0 : (Math.Round((double)((project.ContractValue - project.Cost) / project.ContractValue * 100))),
                        TargetProfit = (project.ContractValue == null || project.EstimetedBudget == 0) ? 0 : (Math.Round((double)((project.ContractValue - project.EstimetedBudget) / project.ContractValue * 100)))

                    });
                }
            }
            return projects;
        }


        public async Task<List<Project>> GetProjectsByManager(int id)
        {
            var projects = new List<Project>();
            var AllProjects = await _context.Projects.Where(x => x.EmployeesId == id && x.DeliveryDate > DateTime.Now).OrderByDescending(x => x.Id).ToListAsync();
            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    projects.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name,
                    });
                }
            }
            return projects;
        }

        public async Task<List<Project>> GetProjectsByType(String type)
        {
            var projects = new List<Project>();
            var AllProjects = await _context.Projects.Where(x => x.Type == type).OrderByDescending(x => x.Id).ToListAsync();
            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    projects.Add(new Project()
                    {
                        Id = project.Id,
                        Description = project.Description,
                        Name = project.Name,
                        Status = project.Status
                    });
                }
            }
            return projects;
        }

        public async Task<List<Project>> GetProjectsByState(String state)
        {
            var projects = new List<Project>();
            if (state == "Good")

            {
                var AllProjects = await _context.Projects.Where(x => (x.DeliveryDate > x.ClosedDate) && (x.EstimetedBudget > x.Cost)).OrderByDescending(x => x.Id).ToListAsync();
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        projects.Add(new Project()
                        {
                            Id = project.Id,
                            Description = project.Description,
                            Name = project.Name,
                            Status = project.Status
                        });
                    }
                }

            }
            else if (state == "Amature")
            {

                var AllProjects = await _context.Projects.Where(x => (x.DeliveryDate == x.ClosedDate) || (x.EstimetedBudget == x.Cost)).OrderByDescending(x => x.Id).ToListAsync();
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        projects.Add(new Project()
                        {
                            Id = project.Id,
                            Description = project.Description,
                            Name = project.Name,
                            Status = project.Status
                        });
                    }
                }

            }
            else
            {

                var AllProjects = await _context.Projects.Where(x => (x.DeliveryDate < x.ClosedDate) || (x.EstimetedBudget < x.Cost)).OrderByDescending(x => x.Id).ToListAsync();
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        projects.Add(new Project()
                        {
                            Id = project.Id,
                            Description = project.Description,
                            Name = project.Name,
                            Status = project.Status
                        });
                    }
                }

            }

            return projects;
        }

        public async Task<List<Project>> GetCurrentProjects()
        {
            var projects = new List<Project>();
            var AllProjects = await _context.Projects.Where(x => x.StartDate < DateTime.Now && x.DeliveryDate > DateTime.Now).OrderByDescending(x => x.Id).ToListAsync();

            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    projects.Add(new Project()
                    {
                        Id = project.Id,
                        Description = project.Description,
                        Name = project.Name,
                        Status = project.Status
                    });
                }
            }
            return projects;
        }


        public async Task<List<Project>> GetClosedProjects()
        {
            var projects = new List<Project>();
            var AllProjects = await _context.Projects.Where(x => x.Status == "Closed").OrderByDescending(x => x.Id).ToListAsync();

            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    projects.Add(new Project()
                    {
                        Id = project.Id,
                        Description = project.Description,
                        Name = project.Name,
                        Status = project.Status
                    });
                }
            }
            return projects;
        }


        public async Task<Project> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                var details = new Project()
                {
                    Id = project.Id,
                    Description = project.Description,
                    Name = project.Name,
                    Type = project.Type,
                    StartDate = project.StartDate,
                    DeliveryDate = project.DeliveryDate,
                    CreatedDate = project.CreatedDate,
                    LastUpdate = project.LastUpdate,
                    ContractValue = project.ContractValue,
                    EstimetedBudget = project.EstimetedBudget,
                    ProjectManagerId = project.EmployeesId,
                    CustomerId = project.CustomersId,
                    Status = project.Status,
                    Cost = project.Cost,
                    AllocatedTasks = project.AllocatedTasks,
                    FinalizedTasks = project.FinalizedTasks,
                    PreProjectId = project.PreProjectId,
                };
                return details;
            }
            return null;

        }

        public async Task<bool> EditProject(Project project)
        {
            var proj = await _context.Projects.FindAsync(project.Id);


            proj.Description = project.Description;
            proj.Name = project.Name;
            proj.Type = project.Type;
            proj.StartDate = project.StartDate;
            proj.DeliveryDate = project.DeliveryDate;
            proj.LastUpdate = DateTime.Now;
            proj.ContractValue = project.ContractValue;
            proj.EstimetedBudget = project.EstimetedBudget;
            proj.EmployeesId = project.ProjectManagerId;
            proj.CustomersId = project.CustomerId;
            proj.Status = project.Status;
            proj.Cost = project.Cost + project.ExtraCost;
            if (project.Status == "Closed")
            {
                proj.ClosedDate = DateTime.Now;
            }


            _context.Entry(proj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<int> AddProject(Project project)
        {

            var NewProject = new Projects
            {
                Description = project.Description,
                Name = project.Name,
                Type = project.Type,
                StartDate = project.StartDate,
                DeliveryDate = project.DeliveryDate,
                CreatedDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                ClosedDate = project.StartDate,
                ContractValue = project.ContractValue,
                EstimetedBudget = project.EstimetedBudget,
                EmployeesId = project.ProjectManagerId,
                CustomersId = project.CustomerId,
                AllocatedTasks = 0,
                FinalizedTasks = 0,
                Cost = 0
            };
            await _context.Projects.AddAsync(NewProject);
            await _context.SaveChangesAsync();

            return NewProject.Id;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> AddToDeleted(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            var Emp = await _context.Employees.FindAsync(project.EmployeesId);
            String CustomerName;
            if (project.CustomersId != null)
            {
                var Cus = await _context.Customers.FindAsync(project.CustomersId);
                CustomerName = Cus.Name;
            }
            else
            {
                CustomerName = "none";
            }


            var NewProject = new DeletedProjects
            {
                Description = project.Description,
                Name = project.Name,
                Type = project.Type,
                StartDate = project.StartDate,
                DeliveryDate = project.DeliveryDate,
                CreatedDate = project.CreatedDate,
                LastUpdate = project.LastUpdate,
                ContractValue = project.ContractValue,
                EstimetedBudget = project.EstimetedBudget,
                ProjectManager = Emp.EmpFName + " " + Emp.EmpLName,
                Status = project.Status,
                Cost = project.Cost,
                CustomerName = CustomerName
            };
            await _context.DeletedProjects.AddAsync(NewProject);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Project>> SearchProject(String Name)
        {

            var projects = new List<Project>();
            var AllProjects = await _context.Projects.OrderByDescending(x => x.Id).ToListAsync();

            if (AllProjects?.Any() == true)
            {
                foreach (var project in AllProjects)
                {
                    if (project.Name.Contains(Name))
                    {
                        projects.Add(new Project()
                        {
                            Id = project.Id,
                            Description = project.Description,
                            Name = project.Name,
                            Status = project.Status
                        });
                    }

                }
            }
            return projects;

        }

        public async Task<List<Status>> ProjectStatus()
        {
            var data = await _context.Projects.ToListAsync();
            int[] arr = new int[4] { 0, 0, 0, 0 };
            string[] arr2 = new string[4] { "Project", "Maintainance", "R and D", "Internal" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (dat.Type == arr2[j])
                    {
                        arr[j]++;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }

        public async Task<List<Status>> ProjectStatusByMonth()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var data = await _context.Projects.Where(x => (x.StartDate.Month == month) && (x.StartDate.Year == year)).ToListAsync();
            int[] arr = new int[4] { 0, 0, 0, 0 };
            string[] arr2 = new string[4] { "Project", "Maintainance", "R and D", "Internal" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (dat.Type == arr2[j])
                    {
                        arr[j]++;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }

        public async Task<List<Status>> ProjectStatusByMonth2()
        {
            var list = new List<Status>();

            var date = DateTime.Now.Date;
            var count = -4;
            while (count < 1)
            {
                var Tasks = await _context.Projects.Where(s => s.StartDate.Date.Month == date.AddMonths(count).Month).CountAsync();


                if (count == 0)
                {
                    list.Add(new Status()
                    {
                        ProjectType = date.AddMonths(count).ToString("MMMM") + "(Current)",
                        Count = Tasks

                    });

                }
                else
                {
                    list.Add(new Status()
                    {
                        ProjectType = date.AddMonths(count).ToString("MMMM"),
                        Count = Tasks

                    });
                }

                count++;
            }

            return list;
        }

        public async Task<List<Status>> ProjectStatus2()
        {
            var data = await _context.Projects.ToListAsync();
            int[] arr = new int[3] { 0, 0, 0 };
            string[] arr2 = new string[3] { "Good", "Amature", "Over Margin" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                if ((dat.DeliveryDate > DateTime.Now) && (dat.EstimetedBudget > dat.Cost))
                {
                    arr[0]++;
                }
                else if ((dat.DeliveryDate == DateTime.Now.Date) || (dat.EstimetedBudget == dat.Cost))
                {
                    arr[1]++;
                }
                else
                {
                    arr[2]++;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }


        public async Task<List<Status>> ProjectStatus3()
        {
            var data = await _context.Projects.ToListAsync();
            int[] arr = new int[2] { 0, 0 };
            string[] arr2 = new string[2] { "Ongoing", "Finalized" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                if (dat.StartDate < DateTime.Now && dat.DeliveryDate > DateTime.Now)
                {
                    arr[0]++;
                }
                else if (dat.Status == "Closed")
                {
                    arr[1]++;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }


        public async Task<List<Status>> ProjectStatus4()
        {
            var data = await _context.Projects.ToListAsync();
            int[] arr = new int[5] { 0, 0, 0, 0, 0 };
            string[] arr2 = new string[5] { "Good", "Amature", "Over Margin-Stage1", "Over Margin-Stage2", "Over Margin-Stage3" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                if (dat.DeliveryDate > DateTime.Now)
                {
                    arr[0]++;
                }
                else if (dat.DeliveryDate == DateTime.Now)
                {
                    arr[1]++;
                }
                else if (DateTime.Now.Subtract(dat.DeliveryDate).TotalDays < 5)
                {
                    arr[2]++;
                }
                else if (DateTime.Now.Subtract(dat.DeliveryDate).TotalDays < 10)
                {
                    arr[3]++;
                }
                else
                {
                    arr[4]++;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }

        public async Task<List<Status>> ProjectStatus5()
        {
            var data = await _context.Projects.ToListAsync();
            int[] arr = new int[5] { 0, 0, 0, 0, 0 };
            string[] arr2 = new string[5] { "Good", "Amature", "Over Margin-Stage1", "Over Margin-Stage2", "Over Margin-Stage3" };
            var projects = new List<Status>();
            foreach (var dat in data)
            {
                if (dat.EstimetedBudget > dat.Cost)
                {
                    arr[0]++;
                }
                else if (dat.EstimetedBudget == dat.Cost)
                {
                    arr[1]++;
                }
                else if ((dat.Cost - dat.EstimetedBudget) < (dat.ContractValue - dat.EstimetedBudget))
                {
                    arr[2]++;
                }
                else if ((dat.Cost - dat.EstimetedBudget) == (dat.ContractValue - dat.EstimetedBudget))
                {
                    arr[3]++;
                }
                else
                    arr[4]++;
            }

            for (int i = 0; i < 5; i++)
            {
                projects.Add(new Status()
                {
                    ProjectType = arr2[i],
                    Count = arr[i]
                });
            }

            return projects;
        }

    }
}

