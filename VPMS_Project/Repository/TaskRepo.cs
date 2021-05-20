using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using VPMS_Project.Data;
using VPMS_Project.Models;

namespace VPMS_Project.Repository
{
    public class TaskRepo
    {
        private readonly EmpStoreContext _context = null;

        public TaskRepo(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<List<TaskModel>> GetTaskListAsync(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.EmployeesId == id) && ((x.TaskComplete == false) || (x.TimeSheet == false)))
                          join b in _context.Projects on a.ProjectsId equals b.Id
                          select new TaskModel()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              ProjectName = b.Name,
                              AllocatedHours = a.AllocatedHours,
                              EmpId = id,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              LastUpdate = a.LastUpdate,
                              CreatedDate = a.CreatedDate,
                              Description = a.Description

                          }).OrderBy(x=> x.StartDate).ToListAsync();
        }

        public async Task<List<EmpModel>> GetEmps(int id)
        {
            return await (from a in _context.Employees.Where(x => (x.Status == "Active") && (x.EmpId!=id))
                          join b in _context.Job on a.JobTitleId equals b.JobId
                          select new EmpModel()
                          {
                              EmpId = a.EmpId,
                              EmpFullName = a.EmpFName + " " + a.EmpLName
                          })
                           .ToListAsync();
        }

        public async Task<List<TaskModel>> GetTask(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.EmployeesId == id) && ((x.TaskComplete == true) || (x.TimeSheet == true)))
                          join b in _context.Projects on a.ProjectsId equals b.Id
                          select new TaskModel()
                          {
                              Id = a.Id,
                              Name = a.Name+" - "+ b.Name,
                          })
                           .ToListAsync();
        }

        public async Task<int> AddVal(int TaskId,int id,int Val,int EmpId)
        {
            var task = await _context.Tasks.FindAsync(TaskId);
            var project = await _context.Projects.FindAsync(task.ProjectsId);
            var GivenEmp = await _context.Employees.FindAsync(EmpId);
            var Work = new WorkQualityModel()
            {
              TaskId= TaskId,
              EmployeesId= id,
              Quality=Val,
              GivenEmpId=EmpId,
              ProjectName= project.Name,
              TaskName=task.Name,
              GivenEmp= GivenEmp.EmpFName+" "+GivenEmp.EmpLName
            };

            await _context.WorkQuality.AddAsync(Work);
            await _context.SaveChangesAsync();

            return Work.Id;
        }

        public async Task<int> AddValForCommunication(int id, int Val, int EmpId)
        {
            var com = new Communication()
            {
                EmployeesId = id,
                CommunicationVal = Val,
                GivenEmpId = EmpId,
            };

            await _context.Communication.AddAsync(com);
            await _context.SaveChangesAsync();

            return com.Id;
        }
        public bool CheckRate(int TaskId, int id,int EmpId)
        {
            bool result = _context.WorkQuality.ToList().Exists(x => (x.TaskId == TaskId) && (x.EmployeesId == id) && (x.GivenEmpId == EmpId));
            return result;
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
                        Name = project.Name,
                    });
                }
            }
            return projects;
        }
        public async Task<List<TaskModel>> GetAllTaskListAsync(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.EmployeesId == id))
                          join b in _context.TimeSheetTask on a.Id equals b.TaskId
                          join c in _context.Projects on a.ProjectsId equals c.Id
                          select new TaskModel()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              ProjectName = c.Name,
                              AllocatedHours = a.AllocatedHours,
                              EmpId = id,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              ActualStartDateTime = b.StartDateTime,
                              ActualEndDateTime = b.EndDateTime,
                              BreakHours = b.BreakHours,
                              TaskComplete = a.TaskComplete,
                              TakenHours = b.TotalHours,
                              LastUpdate = a.LastUpdate,
                              CreatedDate = a.CreatedDate,
                              Description = a.Description

                          }).ToListAsync();
        }

        public async Task<List<TaskModel>> GetAllTaskList2(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.EmployeesId == id) && ((x.TaskComplete == true) || (x.TimeSheet == true)))
                          join b in _context.TimeSheetTask on a.Id equals b.TaskId
                          join c in _context.Projects on a.ProjectsId equals c.Id
                          select new TaskModel()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              ProjectName = c.Name,
                              AllocatedHours = a.AllocatedHours,
                              EmpId = id,
                              TakenHours = b.TotalHours,

                          }).ToListAsync();
        }

        public async Task<List<WorkQModel>> GetTasksForWorkQuality(int id)
        {
            return await (from a in _context.WorkQuality.Where(x => x.EmployeesId == id)
                          select new WorkQModel()
                          {
                              Task = a.TaskName,
                              Project = a.ProjectName,
                              Quality = a.Quality,
                              GivenBy=a.GivenEmp
                          }).ToListAsync();
        }

        public async Task<List<CommunicationModel>> GetCommunication(int id)
        {
            return await (from a in _context.Communication.Where(x => x.EmployeesId == id)
                          select new CommunicationModel()
                          {
                              CommuniId = a.Id,
                              CommunicationVal=a.CommunicationVal

                          }).ToListAsync();
        }

        public async Task<List<TaskModel>> AllTaskListAsync(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.EmployeesId == id))
                          join c in _context.Projects on a.ProjectsId equals c.Id
                          select new TaskModel()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              ProjectName = c.Name,
                              TaskComplete = a.TaskComplete,
                              TimeSheet=a.TimeSheet,
                              AllocatedHours = a.AllocatedHours,
                              EmpId = id,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              LastUpdate = a.LastUpdate,
                              CreatedDate = a.CreatedDate,
                              Description = a.Description

                          }).ToListAsync();
        }
        public async Task<bool> CompleteTask(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            task.TimeSheet = true;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<String> GetName(int id)
        {
            var emp = await _context.Employees.FindAsync(id);            
           return emp.EmpFName+" "+emp.EmpLName ;

        }

        public async Task<bool> AddTaskFromList(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            task.TaskComplete = true;
            task.TimeSheet = false;


            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> NotCompleteTask(int id)
        {

            var task = await _context.Tasks.FindAsync(id);
            task.TaskComplete = false;
            task.TimeSheet = true;

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> CancelAddingTask(int id)
        {

            var task = await _context.Tasks.FindAsync(id);

            task.TaskComplete = false;
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<TimeSheetTaskModel>> AddTaskList(int id)
        {
            return await (from a in _context.TimeSheetTask.Where(x => (x.EmployeesId == id) && (x.AppliedDate == DateTime.Now.Date))
                          join b in _context.Tasks on a.TaskId equals b.Id
                          select new TimeSheetTaskModel()
                          {
                              Id = a.Id,
                              Name = b.Name,
                              AppliedDate = a.AppliedDate,
                              ActualStartDateTime = a.StartDateTime,
                              ActualEndDateTime = a.EndDateTime,
                              BreakHours = a.BreakHours,
                              TotalHours = a.TotalHours,
                              EmployeesId = a.EmployeesId,
                              TaskId = a.TaskId,
                              AllocatedHours = b.AllocatedHours,


                          }).ToListAsync();
        }

        public async Task<int> TImeSheetTaskInsert(int id, DateTime Start, DateTime End)
        {
            var taskInfo = await _context.Tasks.FindAsync(id);
            var info = _context.TimeSheetTask.SingleOrDefault(x => (x.TaskId == id) && (x.AppliedDate == Start.Date) && (x.EmployeesId == taskInfo.EmployeesId));

            if (info == null)
            {
                TimeSpan differ = (TimeSpan)(End - Start);
                Double TotalHours = Math.Round((Double)differ.TotalHours, 2);
                var task = new TimeSheetTask()
                {
                    AppliedDate = Start.Date,
                    StartDateTime = Start,
                    EndDateTime = End,
                    TotalHours = TotalHours,
                    BreakHours = 0,
                    EmployeesId = taskInfo.EmployeesId,
                    TaskId = id

                };

                await _context.TimeSheetTask.AddAsync(task);
                await _context.SaveChangesAsync();
                return task.Id;
            }
            else
            {
                TimeSpan differ1 = (TimeSpan)(End - info.StartDateTime);
                Double TotalHours = differ1.TotalHours;

                TimeSpan differ2 = (TimeSpan)(Start - info.EndDateTime);
                Double breakTime = differ2.TotalHours;
                Double TotalBreakHours = info.BreakHours + breakTime;
                Double TotalEffort = TotalHours - TotalBreakHours;

                info.BreakHours = TotalBreakHours;
                info.EndDateTime = End;
                info.TotalHours = TotalEffort;


                _context.Entry(info).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return info.Id;
            }
        }

        public async Task<TaskModel> GetTaskAsync(int id)
        {
            return await (from a in _context.Tasks.Where(x => (x.Id == id))
                          select new TaskModel()
                          {
                              Id = id,
                              Name = a.Name,
                              EmpId = a.EmployeesId

                          })
                  .FirstOrDefaultAsync();

        }

        public async Task<List<TimeSheetTaskModel>> GetTimeSheet(int id, DateTime date)
        {
            return await (from a in _context.TimeSheetTask.Where(x => (x.EmployeesId == id) && (x.AppliedDate.Date == date.Date))
                          join b in _context.Tasks on a.TaskId equals b.Id
                          join c in _context.Projects on b.ProjectsId equals c.Id
                          select new TimeSheetTaskModel()
                          {
                              Name = b.Name,
                              ProjectName = c.Name,
                              Description = b.Description,
                              AllocatedHours = b.AllocatedHours,
                              ActualStartDateTime = a.StartDateTime,
                              ActualEndDateTime = a.EndDateTime,
                              BreakHours = a.BreakHours,
                              TotalHours = a.TotalHours,


                          })
                  .ToListAsync();

        }

        public double GetTotalHours(int id, DateTime date)
        {
            double total = _context.TimeSheetTask.Where(x => (x.EmployeesId == id) && (x.AppliedDate.Date == date.Date)).Select(x => x.TotalHours).Sum();
            return total;

        }

        public async Task<TodayWorkModel> GetEmpData(int id)
        {
            return await (from a in _context.Employees.Where(x => (x.EmpId == id))
                          select new TodayWorkModel()
                          {
                              EmpName = a.EmpFName + " " + a.EmpLName,
                              PhotoURL = a.ProfilePhoto
                          })
                      .FirstOrDefaultAsync();

        }


        public async Task<List<TimeSheetTaskModel>> GetWorkSheet(DateTime date)
        {
            return await (from a in _context.TimeSheetTask.Where(x => (x.AppliedDate.Date == date.Date))
                          join b in _context.Tasks on a.TaskId equals b.Id
                          join c in _context.Employees on a.EmployeesId equals c.EmpId
                          select new TimeSheetTaskModel()
                          {
                              Name = b.Name,
                              AllocatedHours = b.AllocatedHours,
                              EmpName = c.EmpFName + " " + c.EmpLName,
                              Description = b.Description,
                              ActualStartDateTime = a.StartDateTime,
                              ActualEndDateTime = a.EndDateTime,
                              BreakHours = a.BreakHours,
                              TotalHours = a.TotalHours,


                          })
                  .ToListAsync();

        }


        public async Task<Boolean> StaffTaskInsert(int empId, int projectId, String Task, String Des, DateTime S_Date, DateTime E_Date)
        {
            var taskInfo = await _context.Projects.FindAsync(projectId);
            TimeSpan differ = (TimeSpan)(E_Date - S_Date);
            Double TotalHours = Math.Round((Double)differ.TotalHours, 2);
            var task = new Staff_Task()
            {
                TaskName = Task,
                ProjectName = taskInfo.Name,
                ProjectId = projectId,
                AppliedDate = DateTime.Now,
                StartDate = S_Date,
                EndDate = E_Date,
                TakenHours = TotalHours,
                Description = Des,
                Recommend = "Waiting for recommend",
                EmpId = empId

            };

            await _context.Staff_Task.AddAsync(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> TaskTableInsert(int empId, int projectId, String Task, String Des, DateTime S_Date, DateTime E_Date)
        {
            var EmpInfo = await _context.Employees.FindAsync(empId);
            var taskInfo = await _context.Projects.FindAsync(projectId);
            var managerInfo = await _context.Employees.FindAsync(taskInfo.EmployeesId);
            TimeSpan differ = (TimeSpan)(E_Date - S_Date);
            Double TotalHours = Math.Round((Double)differ.TotalHours, 2);
            var task = new Tasks()
            {
                Name = Task,
                StartDate = S_Date,
                EndDate = E_Date,
                CreatedDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Description = "Non-Allocated Task"+ Des,
                TimeSheet = true,
                ProjectManager = managerInfo.EmpFName,
                ProjectName = taskInfo.Name,
                EmployeeName = EmpInfo.EmpFName,
                ProjectsId = projectId,
                EmployeesId = empId,
                AllocatedHours = TotalHours,
                ProjectManagerId = taskInfo.EmployeesId,
                TaskComplete = true
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task.Id;
        }

        public async Task<List<TimeSheetTaskModel>> GetTaskRecommend()
        {
            return await (from a in _context.Staff_Task.Where(x => x.Recommend == "Waiting for recommend")
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          join c in _context.Projects on a.ProjectId equals c.Id
                          select new TimeSheetTaskModel()
                          {
                              Id = a.Id,
                              Name = a.TaskName,
                              ProjectName = c.Name,
                              EmpName = b.EmpFName + " " + b.EmpLName,
                              AppliedDate = a.AppliedDate,
                              ActualStartDateTime = a.StartDate,
                              ActualEndDateTime = a.EndDate,
                              TotalHours = a.TakenHours,
                              Description = a.Description,

                          })
                   .ToListAsync();
        }

        public async Task<bool> TaskRecomend(int id)
        {
            String content = "Your new task request below has been recommeded";
            await EmailSend2(id, content);

            var TaskInfo = await _context.Staff_Task.FindAsync(id);
            await StatusUpdate(TaskInfo.EmpId, TaskInfo.StartDate);  // Updating status that entering time sheets on timesheetcheck table

            int taskId = await TaskTableInsert(TaskInfo.EmpId, TaskInfo.ProjectId, TaskInfo.TaskName, TaskInfo.Description, TaskInfo.StartDate, TaskInfo.EndDate);
            await TImeSheetTaskInsert(taskId, TaskInfo.StartDate, TaskInfo.EndDate);

            var task = await _context.Staff_Task.FindAsync(id);
            task.Recommend = "Recommended";

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> TaskNotRecomend(int id)
        {
            String content = "Sorry!. Your new task request below has been not recommeded";
            await EmailSend2(id, content);
            var task = await _context.Staff_Task.FindAsync(id);
            task.Recommend = "Rejected";

            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<StaffTaskModel>> GetStaffTaskAsync(int id)
        {
            return await (from a in _context.Staff_Task.Where(x => (x.EmpId == id))
                          join b in _context.Projects on a.ProjectId equals b.Id
                          select new StaffTaskModel()
                          {
                              TaskName = a.TaskName,
                              ProjectName = b.Name,
                              AppliedDate = a.AppliedDate,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              TakenHours = a.TakenHours,
                              Description = a.Description,
                              Recommend = a.Recommend,
                              EmpId = a.EmpId

                          }).ToListAsync();
        }

        public int TaskRecomendCount()
        {
            int count = _context.Staff_Task.Where(x => (x.Recommend == "Waiting for recommend")).Select(x => x.Id).Count();
            return count;

        }

        public bool CheckExist(int id)
        {
            bool result = _context.TimeSheetTask.ToList().Exists(x => (x.TaskId == id));
            return result;
        }

        public double TotalHours(int id)
        {
            double Taken = _context.TimeSheetTask.Where(x => (x.TaskId == id)).Select(x => x.TotalHours).Sum();
            return Taken;
        }

        public DateTime StartDate(int id)
        {
            DateTime Start = _context.TimeSheetTask.Where(x => (x.TaskId == id)).Select(x => x.StartDateTime).Min();
            return Start;
        }

        public DateTime EndDate(int id)
        {
            DateTime End = _context.TimeSheetTask.Where(x => (x.TaskId == id)).Select(x => x.EndDateTime).Max();
            return End;
        }

        public async Task<TaskModel2> TaskListInfo(int id)
        {
            bool result = CheckExist(id);
            DateTime Start, End;
            double Taken;
            if (result == true)
            {
                Start = StartDate(id);
                End = EndDate(id);
                Taken = TotalHours(id);
            }
            else
            {
                Start = DateTime.MinValue;
                End = DateTime.MinValue;
                Taken = 0;
            }

            var task = await _context.Tasks.FindAsync(id);
            var Project = await _context.Projects.FindAsync(task.ProjectsId);
            var EMP = await _context.Employees.FindAsync(Project.EmployeesId);

            return await (from a in _context.Tasks.Where(x => (x.Id == id))
                          select new TaskModel2()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              CreatedDate = a.CreatedDate,
                              LastUpdate = a.LastUpdate,
                              Description = a.Description,
                              AllocatedHours = a.AllocatedHours,
                              TaskComplete = a.TaskComplete,
                              TimeSheet=a.TimeSheet,
                              ActualStartDateTime = Start,
                              ActualEndDateTime = End,
                              TakenHours = Taken,
                              ProjectName = Project.Name,
                              ProjectDes = Project.Description,
                              Type = Project.Type,
                              ProjectStartDate = Project.StartDate,
                              ClosedDate = Project.ClosedDate,
                              ProjectDeliveryDate = Project.DeliveryDate,
                              ProjectCreatedDate = Project.CreatedDate,
                              ProjectLastUpdate = Project.LastUpdate,
                              Status = Project.Status,
                              ProjectManager = EMP.EmpFName + " " + EMP.EmpLName,
                              PMPhotoURL = EMP.ProfilePhoto,

                          }).FirstOrDefaultAsync();
        }

        public bool CheckLeave(int id, DateTime date)
        {
            bool result = _context.LeaveApply.ToList().Exists(x => (x.EmpId == id) && (x.Startdate <= date) && (x.EndDate > date) && (x.Status == "Approved"));
            return result;
        }

      

        public async Task<bool> TimeSEmail(int id)
        {
            bool check = CheckEmailsent(id);
            if (check == true)
            {
                await EmailSend(id);
            }
            return true;
        }

        public async Task<bool> LoggedIn(int id)
        {
            bool result = CheckLeave(id, DateTime.Now);
            if (result == false)
            {
                var info = _context.TimeSheetCheck.SingleOrDefault(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date));
                if (info == null)
                {
                    var timeStatus = new TimeSheetCheck()
                    {
                        Date = DateTime.Now.Date,
                        EmpId = id,
                        TimeSheet = false,
                        EmailSent = false,
                    };

                    await _context.TimeSheetCheck.AddAsync(timeStatus);
                    await _context.SaveChangesAsync();

                }
            }

            return true;
        }

        public async Task<EmpModel> GetCurrentUser(String index)
        {

            return await (from a in _context.Employees.Where(x => x.Index == index)
                          join b in _context.Job on a.JobTitleId equals b.JobId
                          select new EmpModel()
                          {
                              EmpId = a.EmpId,
                              EmpFName = a.EmpFName,
                              EmpLName = a.EmpLName,
                              JobTitleId = a.JobTitleId,
                              JobType = b.JobName,
                              PhotoURL = a.ProfilePhoto

                          })
                  .FirstOrDefaultAsync();

        }
        public async Task<bool> StatusUpdate(int id, DateTime date)
        {
            var info = _context.TimeSheetCheck.SingleOrDefault(x => (x.EmpId == id) && (x.Date == date.Date));
            if (info == null)
            {
                var timeStatus = new TimeSheetCheck()
                {
                    Date = date,
                    EmpId = id,
                    TimeSheet = true,
                };

                await _context.TimeSheetCheck.AddAsync(timeStatus);
                await _context.SaveChangesAsync();

            }
            else
            {
                info.TimeSheet = true;

                _context.Entry(info).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<TimeSCheck>> GetNoTimeskAsync()
        {
            return await (from a in _context.TimeSheetCheck.Where(x => (x.TimeSheet == false))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new TimeSCheck()
                          {
                              EmpId = a.EmpId,
                              EmpName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto,
                              Date = a.Date

                          }).ToListAsync();
        }

        public async Task<bool> EmailSend(int id)
        {

            var data = await GetTimeSdates(id);
            var emp = await _context.Employees.FindAsync(id);
            string to = emp.Email;
            String subject = "Regarding Not Entering time Sheets";

            MailMessage mm = new MailMessage();
            mm.To.Add(to);
            mm.Subject = subject;
            mm.From = new MailAddress("bellvantagecom737@gmail.com");
            mm.Body += " Hi " + emp.EmpFName + ",<br>";
            mm.Body += " You have not entered time sheets for the folowing days.<br><br>";
            mm.Body += " <html>";
            mm.Body += "<body>";
            mm.Body += "<table>";
            foreach (var date in data)
            {
                mm.Body += "<tr>";
                mm.Body += "<td>Date :  </td><td>" + date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year + "</td>";
                mm.Body += "</tr>";
                await EmailSent(date.id);

            }

            mm.Body += "</table>";
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


        public async Task<List<UserEmailOption>> GetTimeSdates(int id)
        {
            var UserEmailOption = new List<UserEmailOption>();
            var AllDates = await _context.TimeSheetCheck.Where(x => (x.EmpId == id) && (x.TimeSheet == false) && (x.Date.Date < DateTime.Now.Date) && (x.EmailSent == false)).ToListAsync();
            if (AllDates?.Any() == true)
            {
                foreach (var dates in AllDates)
                {
                    UserEmailOption.Add(new UserEmailOption()
                    {
                        id = dates.Id,
                        Date = dates.Date,

                    });
                }
            }
            return UserEmailOption;
        }

        public async Task<bool> EmailSent(int id)
        {

            var emp = await _context.TimeSheetCheck.FindAsync(id);
            emp.EmailSent = true;

            _context.Entry(emp).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public bool CheckEmailsent(int id)
        {
            bool result = _context.TimeSheetCheck.ToList().Exists(x => (x.EmpId == id) && (x.TimeSheet == false) && (x.Date.Date < DateTime.Now.Date) && (x.EmailSent == false));
            return result;
        }

        public async Task<bool> EmailSend2(int id, String content)
        {
            var task = await _context.Staff_Task.FindAsync(id);
            var emp = await _context.Employees.FindAsync(task.EmpId);
            string to = emp.Email;
            String subject = "Regarding Your New Task Request";

            MailMessage mm = new MailMessage();
            mm.To.Add(to);
            mm.Subject = subject;
            mm.From = new MailAddress("bellvantagecom737@gmail.com");
            mm.Body += " Hi " + emp.EmpFName + ",<br>";
            mm.Body += " " + content + "<br> ";
            mm.Body += " <html>";
            mm.Body += "<body>";
            mm.Body += "<br><table style='border: 1px solid black'>";
            mm.Body += "<tr>";
            mm.Body += "<td><b>Task Name</b> :  </td><td>" + task.TaskName + "</td>";
            mm.Body += "</tr>";
            mm.Body += "<td><b>Project name </b> :  </td><td>" + task.ProjectName + "</td>";
            mm.Body += "</tr>";
            mm.Body += "<tr>";
            mm.Body += "<td><b>Begin date & Time </b>:  </td><td>" + task.StartDate + "</td>";
            mm.Body += "</tr>";
            mm.Body += "<tr>";
            mm.Body += "<td><b>End date & Time </b>:  </td><td>" + task.EndDate + "</td>";
            mm.Body += "</tr>";
            var timeSpan1 = TimeSpan.FromHours(task.TakenHours);
            int hh1 = timeSpan1.Hours;
            int mm1 = timeSpan1.Minutes;
            String total = hh1 + " h " + mm1 + " min";
            mm.Body += "<tr>";
            mm.Body += "<td><b>Time taken</b> :  </td><td>" + total + "</td>";
            mm.Body += "</tr>";
            mm.Body += "</table>";
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

        public int Total(DateTime date)
        {
            int count = _context.Tasks.Where(x => (x.StartDate.Date <= date.Date) && (x.EndDate.Date >= date.Date)).Select(x => x.Id).Count();
            return count;
        }

        public int Done(DateTime date)
        {
            int count = _context.Tasks.Where(x => (x.StartDate.Date <= date.Date) && (x.EndDate.Date >= date.Date) && (x.TimeSheet == true)).Select(x => x.Id).Count();
            return count;
        }

        public int NotDone(DateTime date)
        {
            int count = _context.Tasks.Where(x => (x.StartDate.Date <= date.Date) && (x.EndDate.Date >= date.Date) && (x.TimeSheet == false)).Select(x => x.Id).Count();
            return count;
        }

        public async Task<List<TaskModel>> OverdueTasks(DateTime date)
        {
            return await (from a in _context.Tasks.Where(x => (x.StartDate.Date <= date.Date) && (x.EndDate.Date >= date.Date) && (x.TimeSheet == false))
                          join b in _context.Projects on a.ProjectsId equals b.Id
                          join c in _context.Employees on a.EmployeesId equals c.EmpId
                          select new TaskModel()
                          {
                              Name=a.Name,
                              StartDate=a.StartDate,
                              EndDate=a.EndDate,
                              AllocatedHours=a.AllocatedHours,
                              EmpName=c.EmpFName+" "+c.EmpLName,
                              ProjectName=b.Name,
                              ProjectManager=a.ProjectManager


                          }).ToListAsync();
        }

        public async Task<List<TaskModel>> PendingTasks()
        {
            return await (from a in _context.Tasks.Where(x => (x.TaskComplete == false) || (x.TimeSheet == false))
                          join b in _context.Projects on a.ProjectsId equals b.Id
                          join c in _context.Employees on a.EmployeesId equals c.EmpId
                          select new TaskModel()
                          {
                              Name = a.Name,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              AllocatedHours = a.AllocatedHours,
                              EmpName = c.EmpFName + " " + c.EmpLName,
                              ProjectName = b.Name,
                              ProjectManager = a.ProjectManager


                          }).ToListAsync();
        }

        public int TotalTask()
        {
            int count = _context.Tasks.Select(x => x.Id).Count();
            return count;
        }

        public int DoneTask()
        {
            int count = _context.Tasks.Where(x => (x.TaskComplete == true) && (x.TimeSheet == true)).Select(x => x.Id).Count();
            return count;
        }

        public int NotDoneTask()
        {
            int count = _context.Tasks.Where(x => (x.TaskComplete == false) || (x.TimeSheet == false)).Select(x => x.Id).Count();
            return count;
        }

        public bool CheckIn(int id)
        {
            var info = _context.TimeTracker.SingleOrDefault(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date));
            if (info == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AttendenceModel> GetWorkLog(int id)
            {
            AttendenceModel am = new AttendenceModel();
            var info = _context.TimeTracker.SingleOrDefault(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date));
            if (info.Status== "Work") 
            {
                am.InTime = (DateTime)info.InTime;
                am.Status = info.Status;
                return am;
            }
            else
            {
                am.InTime = (DateTime)info.InTime;
                am.OutTime = (DateTime)info.OutTime;
                am.TotalHours = info.TotalHours;
                am.Status = info.Status;
                return am;
            }          
            }

        public int CumulativeTotal(int id)
        {
            int count = _context.Tasks.Where(x=> (x.EmployeesId==id)).Select(x => x.Id).Count();
            return count;
        }

        public int CumulativeDone(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.TaskComplete==true) && (x.TimeSheet==true)).Select(x => x.Id).Count();
            return count;
        }

        public int MonthlyTotal(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.StartDate.Month==DateTime.Now.Month)).Select(x => x.Id).Count();
            return count;
        }

        public int MonthlyDone(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.TaskComplete == true) && (x.TimeSheet == true) && (x.StartDate.Month == DateTime.Now.Month)).Select(x => x.Id).Count();
            return count;
        }

        public int Pending(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && ((x.TaskComplete == false) || (x.TimeSheet == false))).Select(x => x.Id).Count();
            return count;
        }

        public int InProgress(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.EndDate.Date >=DateTime.Now.Date) && ((x.TaskComplete == false) || (x.TimeSheet == false))).Select(x => x.Id).Count();
            return count;
        }

        public int Overdue(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.EndDate.Date < DateTime.Now.Date) && ((x.TaskComplete == false) || (x.TimeSheet == false))).Select(x => x.Id).Count();
            return count;
        }

        public int TodayTask(int id)
        {
            int count = _context.Tasks.Where(x => (x.EmployeesId == id) && (x.StartDate.Date==DateTime.Now.Date) &&((x.TaskComplete == false) || (x.TimeSheet == false))).Select(x => x.Id).Count();
            return count;
        }
        public int GetWorkQualitySum(int id)
        {
            int sum = _context.WorkQuality.Where(x => (x.EmployeesId == id)).Select(x => x.Quality).Sum();
            return sum;
        }

        public int GetWorkQualityCount(int id)
        {
            int count = _context.WorkQuality.Where(x => (x.EmployeesId == id)).Select(x => x.Id).Count();
            return count;
        }

        public int GetCommunicationCount(int id)
        {
            int count = _context.Communication.Where(x => (x.EmployeesId == id)).Select(x => x.Id).Count();
            return count;
        }

        public int GetComSum(int id)
        {
            int sum = _context.Communication.Where(x => (x.EmployeesId == id)).Select(x => x.CommunicationVal).Sum();
            return sum;
        }

    }

}
