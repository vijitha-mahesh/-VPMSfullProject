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
    public class AttendenceRepo
    {
        private readonly EmpStoreContext _context = null;

        public AttendenceRepo(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<int> AddRequest(AttendenceModel attendenceModel)
        {
            var attendence = new Attendence()
            {
                Date = attendenceModel.Date,
                InTime = attendenceModel.InTime,
                OutTime = attendenceModel.OutTime,
                TotalHours = attendenceModel.TotalHours,
                BreakingHours = attendenceModel.BreakingHours,
                WorkingHours = attendenceModel.WorkingHours,
                Explanation = attendenceModel.Explanation,
                EmpId = attendenceModel.EmpId,
                Type = "Manual",
                AppliedDate = DateTime.UtcNow,
                Status = "Pending"

            };

            await _context.Attendence.AddAsync(attendence);
            await _context.SaveChangesAsync();

            return attendence.AttendenceId;
        }

        public async Task<List<AttendenceModel>> GetRequest(int id)
        {
            return await (from a in _context.Attendence.Where(x => x.EmpId == id)
                          select new AttendenceModel()
                          {
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              BreakingHours = a.BreakingHours,
                              WorkingHours = a.WorkingHours,
                              Explanation = a.Explanation,
                              Status = a.Status
                          })
                  .ToListAsync();

        }


        public bool CheckExist(int id, DateTime date)
        {
            bool result = _context.Attendence.ToList().Exists(x => (x.EmpId == id) && (x.Date == date.Date));
            return result;
        }

        public async Task<List<AttendenceModel>> GetAttendenceApprove()
        {
            return await (from a in _context.Attendence.Where(x => x.Status == "Pending")
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          join c in _context.Job on b.JobTitleId equals c.JobId
                          select new AttendenceModel()
                          {
                              AttendenceId = a.AttendenceId,
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              BreakingHours = a.BreakingHours,
                              WorkingHours = a.WorkingHours,
                              EmpId = a.EmpId,
                              Explanation = a.Explanation,
                              EmpName = b.EmpFName + " " + b.EmpLName,
                              Designation = c.JobName,
                              AppliedDate = (DateTime)a.AppliedDate



                          })
                   .ToListAsync();
        }

        public async Task<bool> ApproveAttendence(int id, String name)
        {
            String content = "Your Attendence request below has been approved";
            await EmailSend(id, content);
            var attendence = await _context.Attendence.FindAsync(id);
            attendence.Status = "Approved";
            attendence.Approver = name;

            _context.Entry(attendence).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            DateTime date = (DateTime)attendence.Date;
            var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == attendence.EmpId) && (x.Date == date.Date));
            if (info == null)
            {
                var markAttendence = new MarkAttendence()
                {
                    Date = attendence.Date,
                    InTime = attendence.InTime,
                    OutTime = attendence.OutTime,
                    TotalHours = attendence.TotalHours,
                    EmpId = attendence.EmpId,
                    Type = "Manual",
                    Status = "Present",
                    EmailSent = true,

                };

                await _context.MarkAttendence.AddAsync(markAttendence);
                await _context.SaveChangesAsync();

            }
            else
            {
                info.InTime = attendence.InTime;
                info.OutTime = attendence.OutTime;
                info.TotalHours = attendence.TotalHours;
                info.Type = "Manual";
                info.Status = "Present";
                info.EmailSent = true;
                _context.Entry(info).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }

            return true;
        }

        public async Task<bool> NotApproveAttendence(int id, String name)
        {
            String content = "Sorry! Your Attendence request below has been not approved";
            await EmailSend(id, content);
            var attendence = await _context.Attendence.FindAsync(id);
            attendence.Status = "Not Approved";
            attendence.Approver = name;

            _context.Entry(attendence).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            DateTime date = (DateTime)attendence.Date;
            var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == attendence.EmpId) && (x.Date == date.Date));
            if (info == null)
            {
                var markAttendence = new MarkAttendence()
                {
                    Date = attendence.Date,
                    InTime = attendence.InTime,
                    OutTime = attendence.OutTime,
                    TotalHours = attendence.TotalHours,
                    EmpId = attendence.EmpId,
                    Type = "Manual",
                    Status = "Absent",
                    EmailSent = true

                };

                await _context.MarkAttendence.AddAsync(markAttendence);
                await _context.SaveChangesAsync();

            }
            else
            {
                info.InTime = attendence.InTime;
                info.OutTime = attendence.OutTime;
                info.TotalHours = attendence.TotalHours;
                info.Type = "Manual";
                info.Status = "Absent";
                info.EmailSent = true;

                _context.Entry(info).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }

            return true;

        }

        public async Task<bool> EmailSend(int id, String content)
        {
            var att = await _context.Attendence.FindAsync(id);
            var emp = await _context.Employees.FindAsync(att.EmpId);
            string to = emp.Email;
            String subject = "Regarding Your Attendence Request";

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
            mm.Body += "<td><b>Explanation Type</b> :  </td><td>" + att.Explanation + "</td>";
            mm.Body += "</tr>";
            mm.Body += "<td><b>In Time </b> :  </td><td>" + att.InTime + "</td>";
            mm.Body += "</tr>";
            mm.Body += "<tr>";
            mm.Body += "<td><b>Out Time </b>:  </td><td>" + att.OutTime + "</td>";
            mm.Body += "</tr>";
            var timeSpan1 = TimeSpan.FromHours(att.TotalHours);
            int hh1 = timeSpan1.Hours;
            int mm1 = timeSpan1.Minutes;
            String total = hh1 + " h " + mm1 + " min";
            mm.Body += "<tr>";
            mm.Body += "<td><b>Total Hours</b> :  </td><td>" + total + "</td>";
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

        public async Task<List<MarkAttendenceModel>> GetAttInfo(int id, DateTime Month)
        {
            int year = Month.Year;
            int month = Month.Month;

            return await (from a in _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Date.Value.Month == month) && (x.Date.Value.Year == year) && (x.Date != DateTime.Now.Date))
                          select new MarkAttendenceModel()
                          {
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              Type = a.Type,
                              Status = a.Status
                          })
                  .OrderBy(x => x.Date).ToListAsync();

        }


        public async Task<List<MarkAttendenceModel>> GetAttInfo2(int id, DateTime Month)
        {
            int year = Month.Year;
            int month = Month.Month;

            return await (from a in _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Date.Value.Month == month) && (x.Date.Value.Year == year) && (x.Date != DateTime.Now.Date))
                          select new MarkAttendenceModel()
                          {
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              Type = a.Type,
                              Status = a.Status
                          })
                  .OrderBy(x => x.Date).ToListAsync();

        }

        public async Task<List<TodayWorkModel>> TodayWorkersAsync()
        {
            return await (from a in _context.MarkAttendence.Where(x => (x.Date == DateTime.Now.Date) && (x.Status != "Leave On Day"))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new TodayWorkModel()
                          {
                              EmpName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto
                          })
                      .ToListAsync();

        }

        public async Task<List<Key2Model>> SearchLeave1(int id)
        {
            return await (from a in _context.MarkAttendence.Where(x => (x.EmpId == id))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new Key2Model()
                          {
                              AttendenceId = a.MarkAttendenceId,
                              EmpId = a.EmpId,
                              EmpFName = b.EmpFName,
                              EmpFullName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto,
                              RelavantDate = (DateTime)a.Date,
                              Status = a.Status,


                          })
                   .ToListAsync();
        }

        public async Task<List<Key2Model>> SearchLeave2(int id, DateTime date)
        {
            return await (from a in _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Date == date.Date))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new Key2Model()
                          {
                              AttendenceId = a.MarkAttendenceId,
                              EmpId = a.EmpId,
                              EmpFName = b.EmpFName,
                              EmpFullName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto,
                              RelavantDate = (DateTime)a.Date,
                              Status = a.Status,


                          })
                   .ToListAsync();
        }

        public async Task<List<Key2Model>> SearchLeave3(DateTime date)
        {
            return await (from a in _context.MarkAttendence.Where(x => (x.Date == date.Date))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new Key2Model()
                          {
                              AttendenceId = a.MarkAttendenceId,
                              EmpId = a.EmpId,
                              EmpFName = b.EmpFName,
                              EmpFullName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto,
                              RelavantDate = (DateTime)a.Date,
                              Status = a.Status,
                          })
                   .ToListAsync();
        }

        public async Task<MarkAttendenceModel> GetSearchAttendenceAsync(int id)
        {
            return await (from a in _context.MarkAttendence.Where(x => x.MarkAttendenceId == id)
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new MarkAttendenceModel()
                          {
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              Type = a.Type,
                              Status = a.Status,
                              EmpId = a.EmpId,
                              EmpName = b.EmpFName + " " + b.EmpLName,
                          })
                  .FirstOrDefaultAsync();

        }

        public async Task<List<MarkAttendenceModel>> EmpNotUpdate()
        {
            return await (from a in _context.MarkAttendence.Where(x => (x.Status == "Not Complete") || (x.Status == "LoggedIn") || ((x.Status == "Absent") && (x.Type == "Auto")) && (x.Date != DateTime.Now.Date))
                          join b in _context.Employees on a.EmpId equals b.EmpId
                          select new MarkAttendenceModel()
                          {
                              EmpId = a.EmpId,
                              EmpName = b.EmpFName + " " + b.EmpLName,
                              PhotoURL = b.ProfilePhoto,
                              Date = (DateTime)a.Date,
                              Status = a.Status,
                              Type = a.Type
                          })
                   .ToListAsync();
        }

        public async Task<WorkHourModel> GetStandardWorkHours()
        {
            return await (from a in _context.StandardWorkHours
                          select new WorkHourModel()
                          {
                              HourId = a.HourId,
                              NoOfHours = a.NoOfHours
                          })
                  .FirstOrDefaultAsync();

        }

        public async Task<bool> UpdateHour(WorkHourModel workHourModel)
        {

            var hour = await _context.StandardWorkHours.FindAsync(workHourModel.HourId);
            hour.NoOfHours = workHourModel.NoOfHours;
            _context.Entry(hour).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public int AttendenceCount()
        {
            int count = _context.Attendence.Where(x => (x.Status == "Pending")).Select(x => x.AttendenceId).Count();
            return count;

        }

        public int PresentCount(int id)
        {
            int count = _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Status == "Present")).Select(x => x.MarkAttendenceId).Count();
            return count;
        }

        public int AbsentCount(int id)
        {
            int count = _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Status == "Absent") && (x.Type == "Manual")).Select(x => x.MarkAttendenceId).Count();
            return count;
        }

        public int LeaveCount(int id)
        {
            int count = _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Status == "Leave On Day")).Select(x => x.MarkAttendenceId).Count();
            return count;
        }

        public int HoursCount(int id)
        {
            int count = _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Status == "Not Complete")).Select(x => x.MarkAttendenceId).Count();
            return count;
        }

        public int InOutCount(int id)
        {
            int count = _context.MarkAttendence.Where(x => (x.EmpId == id) && ((x.Status == "LoggedIn") || (x.Status == "Absent"))).Select(x => x.MarkAttendenceId).Count();
            return count;
        }

    }
}
