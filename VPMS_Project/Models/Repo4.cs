using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using VPMS_Project.Data;

namespace VPMS_Project.Models
{
    public class Repo4
    {

        private readonly EmpStoreContext _context = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Repo4(EmpStoreContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCount()
        {
            return _context.Projects.Count();
        }


        public async Task<List<ProjectWithCategories>> getlist(string type)
        {

            var AllProjects = await _context.Projects.ToListAsync();


            var categories = new List<ProjectWithCategories>();
            var projectlist1 = new List<Project>();
            var projectlist2 = new List<Project>();
            var projectlist3 = new List<Project>();
            var projectlist4 = new List<Project>();

            if (type == "status")
            {
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        if (project.DeliveryDate > DateTime.Now && project.Cost < project.EstimetedBudget)
                        {
                            projectlist1.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName+" "+ _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                        else if (project.DeliveryDate >= DateTime.Now && project.Cost <= project.EstimetedBudget)
                        {
                            projectlist2.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });

                        }
                        else
                        {
                            projectlist3.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                    }
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Good",
                        Projects = projectlist1
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Amature",
                        Projects = projectlist2
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Over-Margin",
                        Projects = projectlist3
                    });

                }
            }
            else if (type == "category")
            {
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        if (project.Type == "Project")
                        {
                            projectlist1.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                        else if (project.Type == "Maintainance")
                        {
                            projectlist2.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });

                        }
                        else if (project.Type == "R and D")
                        {
                            projectlist3.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });

                        }
                        else
                        {
                            projectlist4.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                    }
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Project",
                        Projects = projectlist1
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Maintainance",
                        Projects = projectlist2
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "R and D",
                        Projects = projectlist3
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Internal",
                        Projects = projectlist4
                    });

                }
            }
            else if (type == "state")
            {
                if (AllProjects?.Any() == true)
                {
                    foreach (var project in AllProjects)
                    {
                        if (project.Status != "Closed")
                        {
                            projectlist1.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                        else
                        {
                            projectlist2.Add(new Project()
                            {
                                Id = project.Id,
                                Description = project.Description,
                                Name = project.Name,
                                Status = project.Status,
                                Type = project.Type,
                                StartDate = project.StartDate,
                                DeliveryDate = project.DeliveryDate,
                                CreatedDate = DateTime.Now,
                                LastUpdate = DateTime.Now,
                                ClosedDate = project.StartDate,
                                ContractValue = project.ContractValue,
                                EstimetedBudget = project.EstimetedBudget,
                                ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                AllocatedTasks = project.AllocatedTasks,
                                FinalizedTasks = project.FinalizedTasks,
                                Cost = project.Cost
                            });
                        }
                    }
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Current",
                        Projects = projectlist1
                    });
                    categories.Add(new ProjectWithCategories()
                    {
                        type = "Finalized",
                        Projects = projectlist2
                    });


                }
            }
            else
            {
                var employees = _context.Employees.Where(x => x.Designation == "project manager").ToList();
                foreach (var employee in employees)
                {
                    var projectlist = new List<Project>();
                    if (AllProjects?.Any() == true)
                    {
                        foreach (var project in AllProjects)
                        {
                            if (project.EmployeesId == employee.EmpId)
                            {
                                projectlist.Add(new Project()
                                {
                                    Id = project.Id,
                                    Description = project.Description,
                                    Name = project.Name,
                                    Status = project.Status,
                                    Type = project.Type,
                                    StartDate = project.StartDate,
                                    DeliveryDate = project.DeliveryDate,
                                    CreatedDate = DateTime.Now,
                                    LastUpdate = DateTime.Now,
                                    ClosedDate = project.StartDate,
                                    ContractValue = project.ContractValue,
                                    EstimetedBudget = project.EstimetedBudget,
                                    ProjectManagerName = _context.Employees.Find(project.EmployeesId).EmpFName + " " + _context.Employees.Find(project.EmployeesId).EmpLName,
                                    CustomerName = project.CustomersId != null ? (_context.Customers.Find(project.CustomersId).Name) : "none",
                                    AllocatedTasks = project.AllocatedTasks,
                                    FinalizedTasks = project.FinalizedTasks,
                                    Cost = project.Cost
                                });
                            }
                        }
                    }
                    categories.Add(new ProjectWithCategories()
                    {
                        type = employee.EmpFName+" "+employee.EmpLName,
                        Projects = projectlist
                    });


                }



            }
            return categories;

        }

        public async Task<int> TotalResources()
        {
            var users = await _userManager.GetUsersInRoleAsync("manager");
            var employees = await _context.Employees.ToListAsync();

            for (int i = 0; i < employees.Count(); i++)
            {

                for (int j = 0; j < users.Count(); j++)
                {
                    if (users[j].UserName == employees[i].Index)
                    {
                        employees.RemoveAt(i);
                        i--;
                        break;
                    }
                }

            }
            return employees.Count();
        }

        public async Task<int> AllocatedResources()
        {
            return await _context.Employees.Where(x => x.PMId != 0).CountAsync();
        }

        public async Task<int> NonAllocatedResources()
        {
            var users = await _userManager.GetUsersInRoleAsync("manager");
            var employees = await _context.Employees.Where(x => x.PMId == 0).ToListAsync();

            for (int i = 0; i < employees.Count(); i++)
            {

                for (int j = 0; j < users.Count(); j++)
                {
                    if (users[j].UserName == employees[i].Index)
                    {
                        employees.RemoveAt(i);
                        i--;
                        break;
                    }
                }

            }
            return employees.Count();
        }

        public async Task<List<Employee>> NonAllocatedResourcesDetailsAsync()
        {
            var list = new List<Employee>();
            var users = await _userManager.GetUsersInRoleAsync("manager");
            var employees =await _context.Employees.Where(x => x.PMId == 0).ToListAsync();
            if (employees?.Any() == true)
            {

                for (int i = 0; i < employees.Count(); i++)
                {

                    for (int j = 0; j < users.Count(); j++)
                    {
                        if (users[j].UserName == employees[i].Index)
                        {
                            employees.RemoveAt(i);
                            i--;
                            break;
                        }
                    }

                }



            foreach (var employee in employees)
                {
                    list.Add(new Employee()
                    {
                        Id = employee.EmpId,
                        Name = employee.EmpFName+" "+employee.EmpLName,
                        Designation = employee.Designation
                    });
                }

            }
            return list;
        }

        public async Task<List<DashboardProjectList>> ProjectCategories1()
        {
            var list = new List<DashboardProjectList>();
            var ongoinglist = new List<Project>();
            var finalizedlist = new List<Project>();
            var index = _httpContextAccessor.HttpContext.User.FindFirst("Index").Value;

            var employee =await _context.Employees.Where(x => x.Index == index).FirstAsync();

            var projects =await _context.Projects.Where(x => x.EmployeesId == employee.EmpId).ToListAsync();

            foreach(var project in projects)
            {
                if(project.DeliveryDate>DateTime.Now && project.StartDate <= DateTime.Now)
                {
                    ongoinglist.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name,
                        DeliveryDate = project.DeliveryDate,
                        Status = project.Status
                    });
                }else if(project.Status == "Closed")
                {
                    finalizedlist.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name,
                        DeliveryDate = project.DeliveryDate,
                        Status = project.Status
                    });
                }
            }

            list.Add(new DashboardProjectList()
            {
                Name = "Ongoing Projects",
                Projects = ongoinglist
            });
            list.Add(new DashboardProjectList()
            {
                Name = "Finalized Projects",
                Projects = finalizedlist
            });

            return list;
        }


        public async Task<List<DashboardProjectList>> ProjectCategories2()
        {
            var list = new List<DashboardProjectList>();
            var list1 = new List<Project>();
            var list2 = new List<Project>();
            var list3 = new List<Project>();
            var list4 = new List<Project>();

            var index = _httpContextAccessor.HttpContext.User.FindFirst("Index").Value;

            var employee = await _context.Employees.Where(x => x.Index == index).FirstAsync();

            var projects = await _context.Projects.Where(x => x.EmployeesId == employee.EmpId).ToListAsync();

            foreach (var project in projects)
            {
                if (project.Type == "Project")
                {
                    list1.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name
                    });
                }
                else if (project.Type == "Maintainance")
                {
                    list2.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name
                    });
                }
                else if (project.Type == "Internal")
                {
                    list3.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name
                    });
                }
                else
                {
                    list4.Add(new Project()
                    {
                        Id = project.Id,
                        Name = project.Name
                    });
                }
            }

            list.Add(new DashboardProjectList()
            {
                Name = "Project",
                Projects = list1
            });
            list.Add(new DashboardProjectList()
            {
                Name = "Maintainance",
                Projects = list2
            });
            list.Add(new DashboardProjectList()
            {
                Name = "Internal",
                Projects = list3
            });
            list.Add(new DashboardProjectList()
            {
                Name = "R and D",
                Projects = list4
            });

            return list;
        }

        public async Task<List<DashboardTaskList>> TaskCategories()
        {
            var list = new List<DashboardTaskList>();
            var list1 = new List<Taskz>();
            var list2 = new List<Taskz>();
            var list3 = new List<Taskz>();
            var list4 = new List<Taskz>();

            var index = _httpContextAccessor.HttpContext.User.FindFirst("Index").Value;

            var employee = await _context.Employees.Where(x => x.Index == index).FirstAsync();

            var Tasks = await _context.Tasks.Where(x => x.ProjectManagerId == employee.EmpId).ToListAsync();

            foreach (var task in Tasks)
            {
                if (task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now)
                {
                    list1.Add(new Taskz()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        project = task.ProjectName,
                        Employee = task.EmployeeName
                    });
                }
                else if (task.StartDate > DateTime.Now)
                {
                    list2.Add(new Taskz()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        project = task.ProjectName,
                        Employee = task.EmployeeName
                    });
                }
                else if (task.TimeSheet == true)
                {
                    list3.Add(new Taskz()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        project = task.ProjectName,
                        Employee = task.EmployeeName
                    });
                }
                else if(task.EndDate < DateTime.Now)
                {
                    list4.Add(new Taskz()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        project = task.ProjectName,
                        Employee = task.EmployeeName
                    });
                }
            }
            
            list.Add(new DashboardTaskList()
            {
                Name = "Ongoing",
                Tasks = list1
            });
            list.Add(new DashboardTaskList()
            {
                Name = "Pending",
                Tasks = list2
            });
            list.Add(new DashboardTaskList()
            {
                Name = "Finalized",
                Tasks = list3
            });
            list.Add(new DashboardTaskList()
            {
                Name = "Timeout",
                Tasks = list4
            });

            return list;
        }

        public async Task<List<Status>> TaskCategories2()
        {
            var list = new List<Status>();
            var index = _httpContextAccessor.HttpContext.User.FindFirst("Index").Value;

            var employee = await _context.Employees.Where(x => x.Index == index).FirstAsync();

            var date = DateTime.Now.Date;
            var count = -6;
            while (count < 1)
            {
                var Tasks = _context.Tasks.Where(s => s.ProjectManagerId == employee.EmpId && s.CreatedDate.Date == date.AddDays(count)).Count();


                if (count == 0)
                {
                    list.Add(new Status()
                    {
                        ProjectType = date.AddDays(count).DayOfWeek.ToString()+"(Today)",
                        Count = Tasks

                    });

                }
                else
                {
                    list.Add(new Status()
                    {
                        ProjectType = date.AddDays(count).DayOfWeek.ToString(),
                        Count = Tasks

                    });
                }
                
               count++;

            }

            //var Tasks  = _context.Tasks.Where(s => s.ProjectManagerId == employee.EmpId).OrderByDescending(s => s.CreatedDate)
            //   .GroupBy(s => s.CreatedDate.Date)
            //   .Select(s => new 
            //   {
            //       ProjectType = s.Key,
            //       Count = s.Count()
            //   }).ToList().Take(5);

           
            return list;
        }


        public async Task<int> TaskToday()
        {
            var tasks = await _context.Tasks.Where(x => x.CreatedDate.Date == DateTime.Now.Date).CountAsync();
            return tasks;
        }

        public async Task<int> TaskToday2()
        {
            var tasks = await _context.Tasks.Where(x => x.StartDate.Date == DateTime.Now.Date || x.EndDate == DateTime.Now.Date).CountAsync();
            return tasks;
        }

        public async Task<int> TaskToday3()
        {
            var tasks = await _context.Tasks.Where(x => (x.StartDate.Date == DateTime.Now.Date || x.EndDate == DateTime.Now.Date)&& x.TimeSheet==true).CountAsync();
            return tasks;
        }

        public async Task<List<Employee>> GetDashboardEmps()
        {
            var index = _httpContextAccessor.HttpContext.User.FindFirst("Index").Value;

           

            var manager = await _context.Employees.Where(x => x.Index == index).FirstAsync();

            var employee = await _context.Employees.Where(x => x.PMId == manager.EmpId).ToListAsync();

            var list = new List<Employee>();
            foreach(var emp in employee)
            {
                list.Add(new Employee()
                {
                    Name = emp.EmpFName+" "+emp.EmpLName,
                    Id = emp.EmpId
                });
            }

            return list;
        }



        public async Task EmailProjectStatus(int id)
        {
            bool attendance = _context.MarkAttendence.ToList().Exists(attendance => attendance.EmpId == id && attendance.Date == DateTime.Today);
            if (attendance == false)
            {
                var emp = await _context.Employees.FindAsync(id);
                var projToday = await _context.Projects.Where(proj => proj.EmployeesId == id && proj.DeliveryDate.Date == DateTime.Today).ToListAsync();
                var project5 = await _context.Projects.Where(proj => proj.EmployeesId == id && proj.DeliveryDate.Date == DateTime.Today.AddDays(5)).ToListAsync();
                var projDelay1 = await _context.Projects.Where(proj => proj.EmployeesId == id && proj.DeliveryDate.Date.AddDays(1) == DateTime.Today).ToListAsync();
                var projDelay5 = await _context.Projects.Where(proj => proj.EmployeesId == id && proj.DeliveryDate.Date.AddDays(5) == DateTime.Today).ToListAsync();
                var projDelay10 = await _context.Projects.Where(proj => proj.EmployeesId == id && proj.DeliveryDate.Date.AddDays(10) == DateTime.Today).ToListAsync();

                if (projToday?.Any() == true)
                {
                    this.SendMail(projToday, "projToday", emp);
                }
                if (project5?.Any() == true)
                {
                    this.SendMail(project5, "projetc5", emp);
                }
                if (projDelay1?.Any() == true)
                {
                    this.SendMail(projDelay1, "projDelay1", emp);
                }
                if (projDelay5?.Any() == true)
                {
                    this.SendMail(projDelay5, "projDelay5", emp);
                }
                if (projDelay10?.Any() == true)
                {
                    this.SendMail(projDelay10, "projDelay10", emp);
                }


            }

            return;
        }

        public bool SendMail(List<Projects> projects, string name, Employees emp)
        {

            string to = emp.Email;
            String subject = "Regarding Project Status Update";

            MailMessage mm = new MailMessage();
            mm.To.Add(to);
            mm.Subject = subject;
            mm.From = new MailAddress("bellvantagecom737@gmail.com");
            mm.Body += " Hi " + emp.EmpFName + ",<br>";
            mm.Body += " There are pending status of projects you need to be consider.<br><br>";
            mm.Body += " <html>";
            mm.Body += "<body>";


            if (name == "projToday")
            {
                mm.Body += "<h3 style='font-weight:bold'> It's already time to deliver following projects </h3>";
                mm.Body += "<h4> Update the status if the projects are ready to be delivered.</h3>";
                mm.Body += "<div><label style='font-weight:bold'>Following projects need your attention</label></div>";

                foreach (var data in projects)
                {
                    mm.Body += "<div><label> Project Name : " + data.Name + " " + (data.Id) + "</label></div>";
                    mm.Body += "<div><label> Project Type : " + data.Type + "</label></div>";
                    mm.Body += "</hr></br>";
                }

            }
            else if (name == "projetc5")
            {
                mm.Body += "<h3 style='font-weight:bold'> 5 more days to deliver following projects </h3>";
                mm.Body += "<h4> Update the status if the projects are ready to be delivered.</h3>";
                mm.Body += "<div><label style='font-weight:bold'>Following projects need your attention</label></div>";

                foreach (var data in projects)
                {
                    mm.Body += "<div><label> Project Name : " + data.Name + " " + (data.Id) + "</label></div>";
                    mm.Body += "<div><label> Project Type : " + data.Type + "</label></div>";
                    mm.Body += "</hr></br>";
                }
            }
            else if (name == "projDelay1")
            {
                mm.Body += "<h3 style='font-weight:bold'> It's already delayed to deliver following projects </h3>";
                mm.Body += "<h4> Update the status if the projects are ready to be delivered.</h3>";
                mm.Body += "<div><label style='font-weight:bold'>Following projects need your attention</label></div>";

                foreach (var data in projects)
                {
                    mm.Body += "<div><label> Project Name : " + data.Name + " " + (data.Id) + "</label></div>";
                    mm.Body += "<div><label> Project Type : " + data.Type + "</label></div>";
                    mm.Body += "</hr></br>";
                }
            }
            else if (name == "projDelay5")
            {
                mm.Body += "<h3 style='font-weight:bold'> It's almost 5 days behind the schedule to deliver following projects </h3>";
                mm.Body += "<h4> Update the status if the projects are ready to be delivered.</h3>";
                mm.Body += "<div><label style='font-weight:bold'>Following projects need your attention</label></div>";

                foreach (var data in projects)
                {
                    mm.Body += "<div><label> Project Name : " + data.Name + " " + (data.Id) + "</label></div>";
                    mm.Body += "<div><label> Project Type : " + data.Type + "</label></div>";
                    mm.Body += "</hr></br>";
                }
            }
            else if (name == "projDelay10")
            {
                mm.Body += "<h3 style='font-weight:bold'> It's almost 10 days behind the schedule to deliver following projects </h3>";
                mm.Body += "<h4> Update the status if the projects are ready to be delivered.</h3>";
                mm.Body += "<div><label style='font-weight:bold'>Following projects need your attention</label></div>";

                foreach (var data in projects)
                {
                    mm.Body += "<div><label> Project Name : " + data.Name + " " + (data.Id) + "</label></div>";
                    mm.Body += "<div><label> Project Type : " + data.Type + "</label></div>";
                    mm.Body += "</hr></br>";
                }
            }

            mm.Body += "</body>";
            mm.Body += "</html>";
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;

            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("bellvantagecom737@gmail.com", "Bell@1234");
            smtp.EnableSsl = true;
            smtp.Send(mm);
            return true;
        }


    }
}
