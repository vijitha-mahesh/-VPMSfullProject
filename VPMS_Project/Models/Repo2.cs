using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Data;
using VPMS_Project.Models;

namespace VPMS_Project.Models
{
    public class Repo2
    {

        private readonly EmpStoreContext _context = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        

        public Repo2(EmpStoreContext context, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public async Task<List<Employee>> GetEmployeeByTitle(String title)
        {
            var data1 = _context.Employees.Where(y => y.Designation == title).OrderBy(x => x.EmpFName);
            var data = await data1.Select(x => new Employee()
            {

                Id = x.EmpId,
                Name = x.EmpFName+" "+x.EmpLName,
                Designation = x.Designation
            }).ToListAsync();

            return data;
        }

        public async Task<List<Employee>> GetDevelopers()
        {
            var data1 = _context.Employees.Where(y => y.Designation != "project manager");
            var data = await data1.Select(x => new Employee()
            {

                Id = x.EmpId,
                Name = x.EmpFName + " " + x.EmpLName,
                Designation = x.Designation
            }).ToListAsync();

            return data;
        }

        public async Task<List<Resources2>> GetEmployees()
        {
            var list1 = new List<String>();
            var list2 = new List<int>();
            var data = new List<Resources2>();
            var data1 = await GetEmployeeByTitle("project manager");
            var data2 = await _context.Employees.Select(x => new Employee()
            {
                projectManagerId = x.PMId,
                Id = x.EmpId,
                Name = x.Designation + " - " + x.EmpFName + " " + x.EmpLName

            }).ToListAsync();

            for(int i=0; i<data1.Count(); i++)
            {
                foreach(var emp in data2)
                {
                    if(emp.projectManagerId == data1[i].Id)
                    {
                        list1.Add(emp.Name);
                        list2.Add(emp.Id);
                    }
                }
                data.Add(new Resources2()
                {
                    ProjectManager = data1[i].Name,
                    employee = new List<string>(list1),
                    Ids = new List<int>(list2),
                    Count = list1.Count()

                }) ;
                list1.Clear();
                list2.Clear();
            }

           
            return data;
        }

        public async Task<String> GetEmployeeNameById(int Id)
        {
            var data = await _context.Employees.FindAsync(Id);

            return data.EmpFName+" "+data.EmpLName;
        }

        public async Task<List<String>> GetEmpoyeesTitles()
        {
            var Titles = new List<String>();
            var AllEmployees = await _context.Employees.ToListAsync();

            if (AllEmployees?.Any() == true)
            {
                foreach (var employee in AllEmployees)
                {
                    if (!Titles.Contains(employee.Designation) && employee.Designation != "project manager")
                    {
                        Titles.Add(employee.Designation);
                    }
                }
            }
            return Titles;
        }

        public List<Employee> GetEmployeesDropdown(List<int> ids)
        {
            var employee = new List<Employee>();

            var selected = _context.Employees.Where(x => ids.Contains(x.EmpId));

            if (selected?.Any() == true)
            {
                foreach (var emp in selected)
                {
                    employee.Add(new Employee()
                    {
                        Id = emp.EmpId,
                        Name = emp.EmpFName + " " + emp.EmpLName + " - " + emp.Designation
                    });
                }
            }
            return employee;
        }

        public async Task<bool> AddCost(int id, DateTime Start, DateTime End)
        {
            var taskInfo = await _context.Tasks.FindAsync(id);
            int projectId = taskInfo.ProjectsId;
            int empId = taskInfo.EmployeesId;
            var emp = await _context.Employees.FindAsync(empId);
            Double rate = await _context.Salarys.Where(x => x.Designation == emp.Designation).Select(y => y.HourlyRate).FirstOrDefaultAsync();
            Double hours = (End.ToLocalTime() - Start.ToLocalTime()).TotalHours;

            var proj = await _context.Projects.FindAsync(projectId);
            proj.Cost = proj.Cost + (rate * hours);
            proj.FinalizedTasks = proj.FinalizedTasks + 1;

            _context.Entry(proj).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeductCost(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task.StartDate > DateTime.UtcNow)
            {
                var emp = await _context.Employees.FindAsync(task.EmployeesId);

                Double rate = await _context.Salarys.Where(x => x.Designation == emp.Designation).Select(y => y.HourlyRate).FirstAsync();
                Double hours = (task.EndDate.ToLocalTime() - task.StartDate.ToLocalTime()).TotalHours;

                var proj = await _context.Projects.FindAsync(task.ProjectsId);

                proj.Cost = proj.Cost - rate * hours;

                _context.Entry(proj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<int> TaskCount(int id)
        {
            var count = await _context.PreSalesTasks.Where(task => task.ProjectsID == id).CountAsync();
            return count;
        }

        public async Task<bool> AddSalaryInfo(Salary salary)
        {
            var sal = new Salarys
            {
                Designation = salary.Designation,
                HourlyRate = salary.HourlyRate
            };

            await _context.Salarys.AddAsync(sal);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<JobModel>> GetTitles()
        {
            var data = new List<JobModel>();

            var list =await _context.Job.ToListAsync();
            var list2 = await _context.Salarys.ToListAsync();
            
            foreach(var dat in list)
            {
              int count=0;
                foreach(var dat2 in list2)
                {
                    if(dat2.Designation != dat.JobName)
                    {
                       count++;
                    }
                    if (count == list2.Count())
                    {
                        data.Add(new JobModel()
                      {
                        JobName = dat.JobName,
                        JobId = dat.JobId
                      });
                    }
                }
                
            }
            return data;
        }

        public async Task<bool> EditSalaryInfo(Salary salary)
        {
            var sal = await _context.Salarys.FindAsync(salary.Id);

            sal.HourlyRate = salary.HourlyRate;

            _context.Entry(sal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Salary>> GetSalaryInfo()
        {
            var data = await _context.Salarys.Select(x => new Salary()
            {
                Id = x.Id,
                Designation = x.Designation,
                HourlyRate = x.HourlyRate
            }).ToListAsync();

            return data;
        }

        public async Task<Salary> GetRateById(int id)
        {
            var sal = await _context.Salarys.FindAsync(id);

            if (sal != null)
            {
                var details = new Salary()
                {
                    Id = sal.Id,
                    Designation = sal.Designation,
                    HourlyRate = sal.HourlyRate
                };
                return details;
            }
            return null;
        }

        public async Task<bool> DeleteSalary(int id)
        {
            var sal = await _context.Salarys.FindAsync(id);

            _context.Salarys.Remove(sal);
            await _context.SaveChangesAsync();
            return true;

        }
        [ResponseCache(Duration =0, NoStore = true)]
        public async Task<int> AddTask(Taskz Task)
        {
            var employee = await _context.Employees.FindAsync(Task.EmployeeId);
            var project = await _context.Projects.FindAsync(Task.projectId);
            int id = Int16.Parse(Task.Name);
            var task = await _context.PreSalesTasks.FindAsync(id);
            var NewTask = new Tasks
            {
                Description = Task.Description,
                Name = task.Description,
                StartDate = Task.StartDate,
                EndDate = Task.EndDate,
                ProjectsId = Task.projectId,
                CreatedDate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                EmployeesId = Task.EmployeeId,
                EmployeeName = employee.EmpFName+" "+employee.EmpLName,
                ProjectName = project.Name,
                ProjectManager = Task.ProjectManager,
                ProjectManagerId = Task.ProjectManagerId,
                TimeSheet = false

            };
            task.IsAssigned = true;
            _context.Entry(task).State = EntityState.Modified;
            project.AllocatedTasks = project.AllocatedTasks + 1;
            _context.Entry(project).State = EntityState.Modified;
            await _context.Tasks.AddAsync(NewTask);
            await _context.SaveChangesAsync();

            return NewTask.Id;
        }

        public async Task<List<int>> SelectEmployees(SelectEmployees emp)
        {
            var empTask = await _context.Tasks.ToListAsync();
            var employees = await _context.Employees.Where(x => x.PMId == emp.ProjectManagerId).ToListAsync();

            var IdList = new List<int>();

            if (empTask?.Any() == true)
            {
                foreach (var task in empTask)
                {
                    if (((emp.StartDate >= task.StartDate) && (emp.StartDate <= task.EndDate)) || ((emp.EndDate >= task.StartDate) && (emp.EndDate <= task.EndDate)) || ((emp.StartDate <= task.StartDate) && (emp.EndDate >= task.EndDate)))
                    {
                        IdList.Add(task.EmployeesId);
                    }
                }
            }
            var list = employees.Where(x => !IdList.Contains(x.EmpId)).Select(y => y.EmpId).ToList();
            return list;

        }

        public async Task<TaskCount> TaskOverview()
        {
            var tasks = new List<Taskz>();
            var AllTasks = await _context.Tasks.ToListAsync();

            if (AllTasks?.Any() == true)
            {
                foreach (var task in AllTasks)
                {
                    tasks.Add(new Taskz()
                    {
                        TimeSheet =task.TimeSheet,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate
                    });
                }
            }
            int total = tasks.Count();
            int ongoing = 0;
            int timeout = 0;
            int finalized = 0;
            int today = 0;
            int pending = 0;

           foreach(var task in tasks)
            {
                if (task.TimeSheet == true)
                {
                    finalized++;
                }
                if(task.EndDate < DateTime.Now)
                {
                    timeout++;
                }
                if(task.StartDate.Date == DateTime.Today || task.EndDate.Date == DateTime.Today)
                {
                    today++;
                }
                if(task.StartDate > DateTime.Now)
                {
                    pending++;
                }
                if(task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now)
                {
                    ongoing++;
                }
            }

            var count = new TaskCount();
            count.Total = total;
            count.Today = today;
            count.Ongoing = ongoing;
            count.Pending = pending;
            count.Finalized = finalized;
            count.TimeOut = timeout;

            return count;

        }

        public async Task<TaskCount> TaskOverview2()
        {
           
            var AllTasks = await _context.Tasks.Where(task=> task.StartDate.Date == DateTime.Today || task.EndDate.Date == DateTime.Today).ToListAsync();
            var count = new TaskCount();
            if (AllTasks?.Any() == true)
            {
                      
            int total = AllTasks.Count();
            int ongoing = 0;
            int timeout = 0;
            int finalized = 0;
            int pending = 0;

                foreach (var task in AllTasks)
                {
                    if (task.TimeSheet == true)
                    {
                        finalized++;
                    }
                    if (task.EndDate < DateTime.Now)
                    {
                        timeout++;
                    }
                    if (task.StartDate > DateTime.Now)
                    {
                        pending++;
                    }
                    if (task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now)
                    {
                        ongoing++;
                    }
                }
            

            count.Total = total;
            count.Ongoing = ongoing;
            count.Pending = pending;
            count.Finalized = finalized;
            count.TimeOut = timeout;
         }
            return count;

        }

        public List<PreTaskModel> GetTaskByProjectId(int id)
        {
            var list = new List<PreTaskModel>();
            var data = _context.PreSalesTasks.Where(task => task.ProjectsID == id && task.IsAssigned == false).ToList();
            foreach(var task  in data)
            {
                list.Add(new PreTaskModel
                {
                    Id = task.Id,
                    Description = task.Description
                });
            }
            return list;
        }

        public IQueryable<Tasks> GetTasks2Async(int id)
        {
            
            if (id == 0)
            {
              var  data1 = _context.Tasks.AsQueryable();
                return data1;
            }
            else
            {
               var data2 = _context.Tasks.Where(task => task.ProjectsId == id);
               return data2;
            }
             
            

        }

        public async Task<List<Taskz>> GetTasks(int count, int emp, int proj, String state)
        {
            var tasks = new List<Taskz>();
            var AllTasks = await _context.Tasks.OrderByDescending(x=> x.Id).ToListAsync() ;
            
           
                if (AllTasks?.Any() == true)
                {

                    if ((emp == 0) && (proj == 0) && (state == null))
                    {

                        foreach (var task in AllTasks)
                        {
                            tasks.Add(new Taskz()
                            {
                                Id = task.Id,
                                Description = task.Description,
                                Name = task.Name,
                                StartDate = task.StartDate,
                                EndDate = task.EndDate,
                                project = task.ProjectName,
                                Employee = task.EmployeeName,
                                CreatedDate = task.CreatedDate,
                                TimeSheet = task.TimeSheet,
                                LastUpdate =task.LastUpdate,
                                ProjectManager =task.ProjectManager
                            });
                        }
                    }
                    else if ((emp == 0) && (proj == 0))
                    {
                        if (state.Equals("finalized"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.TimeSheet == true)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("timeout"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate < DateTime.Now)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                         project = task.ProjectName,
                                         Employee = task.EmployeeName,
                                          CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("today"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.StartDate.Date == DateTime.Today || task.EndDate.Date == DateTime.Today)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("pending"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.StartDate > DateTime.Now )
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("ongoing"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }


                    }
                    else if (state == null)
                    {
                        if (proj == 0)
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        else
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }

                    }
                    else if (emp == 0)
                    {
                        if (state.Equals("finalized"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.TimeSheet == true && task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("timeout"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate < DateTime.Now && task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("today"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if ((task.StartDate.Date == DateTime.Today || task.EndDate.Date == DateTime.Today)&& task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("pending"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.StartDate > DateTime.Now && task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("ongoing"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now && task.ProjectsId == proj)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                    }

                    else
                    {
                        if (state.Equals("finalized"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.TimeSheet == true && task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("timeout"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate < DateTime.Now && task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("today"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if ((task.StartDate.Date == DateTime.Today || task.EndDate.Date == DateTime.Today) && task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("pending"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.StartDate > DateTime.Now && task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                        if (state.Equals("ongoing"))
                        {
                            foreach (var task in AllTasks)
                            {
                                if (task.EndDate >= DateTime.Now && task.StartDate <= DateTime.Now && task.EmployeesId == emp)
                                {
                                    tasks.Add(new Taskz()
                                    {
                                        Id = task.Id,
                                        Description = task.Description,
                                        Name = task.Name,
                                        StartDate = task.StartDate,
                                        EndDate = task.EndDate,
                                        project = task.ProjectName,
                                        Employee = task.EmployeeName,
                                        CreatedDate = task.CreatedDate,
                                        TimeSheet = task.TimeSheet,
                                        LastUpdate = task.LastUpdate,
                                        ProjectManager = task.ProjectManager
                                    });
                                }
                            }
                        }
                    }

            }
            if (count != 0)
            {
                return tasks.Take(count).ToList();
            }
            else
            {
                return tasks;
            }

            
        }

     
        public async Task<Taskz> GetTasksById(int id)
        {
            var data = await _context.Tasks.FindAsync(id);

            var data1 = new Taskz
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                EmployeeId = data.EmployeesId,
                projectId = data.ProjectsId,
                CreatedDate = data.CreatedDate,
                LastUpdate = data.LastUpdate

            };

            return data1;

        }

        public async Task<List<PmTask>> TodayAllocatedTasks()
        {
            var pmtasks = new List<PmTask>();
            var data =await _context.Tasks.Where(x => x.CreatedDate.Date == DateTime.Today).ToListAsync();
            var managers = await _context.Employees.Where(x => x.Designation == "project manager").OrderBy(x => x.EmpFName).ToListAsync();

            if ( (managers?.Any() == true))
            {

                foreach(var manager in managers)
                {
                    var tasks = new List<Taskz>();

                 if  (data?.Any() == true)
                    {
                  foreach (var task in data)
                  {
                        if (task.ProjectManagerId == manager.EmpId)
                        {
                           tasks.Add(new Taskz()
                           {
                              Id = task.Id,
                              Description = task.Description,
                              Name = task.Name,
                              StartDate = task.StartDate,
                              EndDate = task.EndDate,
                              project = task.ProjectName,
                              Employee = task.EmployeeName,
                              CreatedDate = task.CreatedDate,
                              TimeSheet = task.TimeSheet,
                              LastUpdate = task.LastUpdate,
                          });
                        }  
            
                      }
                    }
                  
                     pmtasks.Add(new PmTask()
                        {
                            Name = manager.EmpFName+" "+manager.EmpLName,
                            Tasks = tasks
                        });

                }
            }
            return pmtasks;

        }


        public async Task<bool> EditTask(Taskz tasks)
        {
            var task = await _context.Tasks.FindAsync(tasks.Id);


            task.Description = tasks.Description;
            task.Name = tasks.Name;
            task.LastUpdate = DateTime.UtcNow;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> AddToDeletedTasks(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            var Emp = await _context.Employees.FindAsync(task.EmployeesId);

            var proj = await _context.Projects.FindAsync(task.ProjectsId);

            var NewTask = new DeletedTasks
            {
                Description = task.Description,
                Name = task.Name,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                ProjectsName = proj.Name,
                CreatedDate = task.CreatedDate,
                LastUpdate = task.LastUpdate,
                EmployeesName = Emp.EmpFName+" "+Emp.EmpLName
            };

            await _context.DeletedTasks.AddAsync(NewTask);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<Taskz>> SearchTask(String Name)
        {

            var tasks = new List<Taskz>();
            var AllTasks = await _context.Tasks.OrderByDescending(x => x.Id).ToListAsync();

            if (AllTasks?.Any() == true)
            {
                foreach (var task in AllTasks)
                {
                    if (task.Name.Contains(Name))
                    {
                        tasks.Add(new Taskz()
                        {
                            Id = task.Id,
                            Description = task.Description,
                            Name = task.Name,
                            StartDate = task.StartDate,
                            EndDate = task.EndDate
                        });
                    }

                }
            }
            return tasks;

        }

        public async Task<List<int>> SelectResources(SelectEmployees emp)
        {
            var empTask = await _context.Tasks.ToListAsync();

            var IdList = new List<int>();

            if (empTask?.Any() == true)
            {
                foreach (var task in empTask)
                {
                    if (((emp.StartDate >= task.StartDate) && (emp.StartDate <= task.EndDate)) || ((emp.EndDate >= task.StartDate) && (emp.EndDate <= task.EndDate)) || ((emp.StartDate <= task.StartDate) && (emp.EndDate >= task.EndDate)))
                    {
                        IdList.Add(task.EmployeesId);
                    }
                }
            }

            return IdList;

        }

        public async Task<List<Employee>> GetResourcesAsync(List<int> ids)
        {
            var employee = new List<Employee>();
            var users = await _userManager.GetUsersInRoleAsync("manager");

            var AllEmployees = _context.Employees.ToList();
           

            for (int i = 0; i < AllEmployees.Count(); i++)
            {

                for (int j = 0; j < users.Count(); j++)
                {
                    if (users[j].UserName == AllEmployees[i].Index)
                    {
                        AllEmployees.RemoveAt(i);
                        i--;
                        break;
                    }
                }

            }
            string manager;
           var selected = AllEmployees.Where(x => !ids.Contains(x.EmpId)).OrderBy(x => x.Designation);

            if (selected?.Any() == true)
            {
                foreach (var emp in selected)
                {
                    if (emp.PMId != 0)
                    {
                     var managerData =await _context.Employees.FindAsync(emp.PMId);
                        manager = managerData.EmpFName + " " + managerData.EmpLName;
                    }
                    else
                    {
                        manager = "none";
                    }

                    employee.Add(new Employee()
                    {
                        Id = emp.EmpId,
                        Name = emp.EmpFName + " " + emp.EmpLName,
                        Designation = emp.Designation,
                        ProjectManager = manager
                    }); ;
                }
            }
            return employee;

        }

        public async Task<Resources> AllocateResources1()
        {
            var users =await _userManager.GetUsersInRoleAsync("manager");

            var list = new List<SelectListItem>();
           

            var data = await _context.Employees.Where(y => (y.PMId == 0)).OrderBy(x => x.Designation).ToListAsync();

            if (data?.Any() == true)
            {

              for (int i=0; i<data.Count(); i++) {

                for (int j=0; j < users.Count();  j++)
                {
                    if(users[j].UserName == data[i].Index)
                    {
                        data.RemoveAt(i);
                        i--;
                        break;
                    }
                }

              }
            }
                

            foreach(var emp in data)
            {
                list.Add(new SelectListItem()
                {
                    Text = emp.Designation + " - " + emp.EmpFName + " " + emp.EmpLName,
                    Value = emp.EmpId.ToString()
                });
            }


            var resources = new Resources()
            {
                employee =list,
                Id=0
            };
            return resources;

        }

        public async Task<List<DesignationCount>> DesignationCount()
        {
            var list = new List<DesignationCount>();
           
            var users = await _userManager.GetUsersInRoleAsync("manager");
            var jobs = await _context.Job.ToListAsync();
            var data = await _context.Employees.Where(y => (y.PMId == 0) && (y.Designation != "project manager")).ToListAsync();
            
            if(jobs?.Any() == true)
            {



                for (int i = 0; i < jobs.Count(); i++)
                {

                    for (int j = 0; j < users.Count(); j++)
                    {
                        if (users[j].Designation == jobs[i].JobName)
                        {
                            jobs.RemoveAt(i);
                            i--;
                            break;
                        }
                    }

                }


            }


            if (data?.Any() == true)
            {

                for (int i = 0; i < data.Count(); i++)
                {

                    for (int j = 0; j < users.Count(); j++)
                    {
                        if (users[j].UserName == data[i].Index)
                        {
                            data.RemoveAt(i);
                            i--;
                            break;
                        }
                    }

                }


                foreach (var job in jobs)
                {
                    int count = 0;
                    foreach(var emp in data)
                    {
                        
                        if (job.JobName == emp.Designation)
                        {
                            count++;
                        }
                    }
                    list.Add(new DesignationCount()
                    {
                        Designation = job.JobName,
                        Count = count
                    });
                }
            }


            


            return list;
        }

        public async Task<bool> AllocateResources2(Resources resources)
        {
            var ids =resources.employee.Where(x => x.Selected == true).Select(y => int.Parse(y.Value)).ToList();

            foreach(int id in ids)
            {
                var data = await _context.Employees.FindAsync(id);
                data.PMId = resources.Id;
                _context.Entry(data).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveAllocation(int id)
        {
            var data = await _context.Tasks.Where(x => x.EmployeesId == id).ToListAsync();
            var emp = await _context.Employees.FindAsync(id);
            if(data?.Any() == true)
            {
                foreach(var task in data)
                {
                    if(task.EndDate >= DateTime.Now)
                    {
                        return false;
                    }
                }

            }

            emp.PMId = 0;
            _context.Entry(emp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}