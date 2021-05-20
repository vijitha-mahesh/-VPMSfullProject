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
    public class TimeTrackRepo
    {
        private readonly EmpStoreContext _context = null;

        public TimeTrackRepo(EmpStoreContext context)
        {
            _context = context;
        }

        public async Task<int> InTimeMark(TimeTrackerModel timeTrackerModel)
        {
            var timeTracker = new TimeTracker()
            {
                Date = DateTime.Now.Date,
                InTime = DateTime.Now,
                TotalHours = 0,
                WorkingHours = 0,
                BreakingHours = 0,
                EmpId = timeTrackerModel.EmpId,
                Status = "Work",
                Inlocation=timeTrackerModel.Inlocation,
                Inlatitude= timeTrackerModel.Inlatitude,
                InLongitude= timeTrackerModel.InLongitude

            };

            await _context.TimeTracker.AddAsync(timeTracker);
            await _context.SaveChangesAsync();

            var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == timeTrackerModel.EmpId) && (x.Date == DateTime.Now.Date));
            if (info == null)
            {
                var markAttendence = new MarkAttendence()
                {
                    Date = DateTime.Now.Date,
                    InTime = DateTime.Now,
                    OutTime = null,
                    TotalHours = 0,
                    EmpId = timeTrackerModel.EmpId,
                    Type = "Auto",
                    Status = "Absent",
                    EmailSent = false

                };

                await _context.MarkAttendence.AddAsync(markAttendence);
                await _context.SaveChangesAsync();

            }
            else
            {
                info.InTime = DateTime.Now;
                info.Type = "Auto";
                info.Status = "Absent";
                info.EmailSent = false;

                _context.Entry(info).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }

            return timeTracker.TrackId;
        }

        public async Task<bool> UpdateTrack(TimeTrackerModel timeTrackerModel)
        {

            var Hours = await _context.StandardWorkHours.FindAsync(1);
            var track = await _context.TimeTracker.FindAsync(timeTrackerModel.TrackId);
            track.OutTime = DateTime.Now;
            track.TotalHours = timeTrackerModel.TotalHours;
            track.WorkingHours = timeTrackerModel.WorkingHours;
            track.Status = "Finish";
            track.Type = "Auto";
            track.Outlocation = timeTrackerModel.Outlocation;
            track.Outlatitude = timeTrackerModel.Outlatitude;
            track.OutLongitude = timeTrackerModel.OutLongitude;

            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            if (timeTrackerModel.TotalHours >= Hours.NoOfHours)
            {
                var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == track.EmpId) && (x.Date == DateTime.Now.Date));
                if (info == null)
                {
                    var markAttendence = new MarkAttendence()
                    {
                        Date = DateTime.Now.Date,
                        InTime = track.InTime,
                        OutTime = DateTime.Now,
                        TotalHours = timeTrackerModel.TotalHours,
                        EmpId = track.EmpId,
                        Type = "Auto",
                        Status = "Present",
                        EmailSent = true,

                    };

                    await _context.MarkAttendence.AddAsync(markAttendence);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    info.OutTime = DateTime.Now;
                    info.TotalHours = timeTrackerModel.TotalHours;
                    info.Type = "Auto";
                    info.Status = "Present";
                    info.EmailSent = true;


                    _context.Entry(info).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                }
            }
            else
            {
                var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == track.EmpId) && (x.Date == DateTime.Now.Date));
                if (info == null)
                {
                    var markAttendence = new MarkAttendence()
                    {
                        Date = DateTime.Now.Date,
                        InTime = track.InTime,
                        OutTime = DateTime.Now,
                        TotalHours = timeTrackerModel.TotalHours,
                        EmpId = track.EmpId,
                        Type = "Auto",
                        Status = "Not Complete",
                        EmailSent = false

                    };

                    await _context.MarkAttendence.AddAsync(markAttendence);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    info.OutTime = DateTime.Now;
                    info.Type = "Auto";
                    info.Status = "Not Complete";
                    info.TotalHours = timeTrackerModel.TotalHours;
                    info.EmailSent = false;

                    _context.Entry(info).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                }
            }
            return true;

        }

        public async Task<TimeTrackerModel> GetTime(int id)
        {
            return await (from a in _context.TimeTracker.Where(x => x.EmpId == id && x.Date == DateTime.Now.Date)
                          select new TimeTrackerModel()
                          {
                              TrackId = a.TrackId,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              BreakingHours = a.BreakingHours,
                              EmpId = a.EmpId,
                              Status = a.Status

                          })
                  .FirstOrDefaultAsync();

        }


        public async Task<TimeTrackerModel> TrackInfo(int id)
        {
            return await (from a in _context.TimeTracker.Where(x => x.TrackId == id)
                          select new TimeTrackerModel()
                          {
                              TrackId = a.TrackId,
                              Date= (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              TotalHours = a.TotalHours,
                              BreakingHours = a.BreakingHours,
                              WorkingHours = a.WorkingHours,
                              EmpId = a.EmpId,
                              Status = a.Status,
                              Inlocation = a.Inlocation,
                              Inlatitude = a.Inlatitude,
                              InLongitude = a.InLongitude,
                              Outlocation = a.Outlocation,
                              Outlatitude = a.Outlatitude,
                              OutLongitude = a.OutLongitude,

                          })
                  .FirstOrDefaultAsync();

        }

        public bool CheckExist(int id)
        {
            bool result = _context.TimeTracker.ToList().Exists(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date));
            return result;
        }

        public bool CheckIn(int id)
        {
            bool result = _context.TimeTracker.ToList().Exists(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date) && (x.InTime != null));
            return result;
        }

        public bool CheckOut(int id)
        {
            bool result = _context.TimeTracker.ToList().Exists(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date) && (x.OutTime == null));
            return result;
        }

        public async Task<bool> CheckOut1(int id)
        {
            var track = await _context.TimeTracker.FindAsync(id);
            if (track.OutTime == null && track.Status != "Break")
                return true;
            else
                return false;
        }

        public async Task<bool> CheckOut2(int id)
        {
            var track = await _context.TimeTracker.FindAsync(id);
            if (track.OutTime == null && track.Status == "Break")
                return true;
            else
                return false;
        }


        public bool CheckBreak(int id)
        {
            bool result = _context.TimeTracker.ToList().Exists(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date) && (x.Status == "Break"));
            return result;
        }

        public async Task<bool> StartBreak(int id)
        {

            var track = await _context.TimeTracker.FindAsync(id);
            track.BreakStart = DateTime.Now;
            track.Status = "Break";

            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EndBreak(TimeTrackerModel timeTrackerModel)
        {

            var track = await _context.TimeTracker.FindAsync(timeTrackerModel.TrackId);
            track.BreakEnd = DateTime.Now;
            track.BreakingHours = timeTrackerModel.BreakingHours + track.BreakingHours;
            track.Status = "Work";

            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<TimeTrackerModel>> TrackInfoById(int id)
        {
            return await (from a in _context.TimeTracker.Where(x => x.EmpId == id)
                          select new TimeTrackerModel()
                          {
                              TrackId = a.TrackId,
                              Date = (DateTime)a.Date,
                              InTime = (DateTime)a.InTime,
                              OutTime = (DateTime)a.OutTime,
                              Inlocation=a.Inlocation,
                              Inlatitude=a.Inlatitude,
                              InLongitude=a.InLongitude,
                              Outlocation=a.Outlocation,
                              Outlatitude=a.Outlatitude,
                              OutLongitude=a.OutLongitude,
                              TotalHours = a.TotalHours,
                              BreakingHours = a.BreakingHours,
                              WorkingHours = a.WorkingHours,
                              Status = a.Status
                          })
                  .ToListAsync();
        }

        public async Task<bool> LoggedIn(int id)
        {
            var info = _context.MarkAttendence.SingleOrDefault(x => (x.EmpId == id) && (x.Date == DateTime.Now.Date));
            if (info == null)
            {
                var markAttendence = new MarkAttendence()
                {
                    Date = DateTime.Now.Date,
                    InTime = null,
                    OutTime = null,
                    TotalHours = 0,
                    EmpId = id,
                    Status = "LoggedIn",
                    EmailSent = false,

                };

                await _context.MarkAttendence.AddAsync(markAttendence);
                await _context.SaveChangesAsync();

            }
            return true;
        }

        public async Task<bool> AttendenceEmail(int id)
        {
            bool check = CheckEmailsent(id);
            if (check == true)
            {
                await EmailSend(id);
            }
            return true;
        }

        public bool CheckEmailsent(int id)
        {
            bool result = _context.MarkAttendence.ToList().Exists(x => (x.EmpId == id) && (x.Date < DateTime.Now.AddDays(-1)) &&
            (x.EmailSent == false) && ((x.Status == "Not Complete") || (x.Status == "LoggedIn") || (x.Status == "Absent" && x.Type == "Auto")));
            return result;
        }

        public async Task<bool> EmailSend(int id)
        {
            var data = await GetTimeSdates(id);
            var emp = await _context.Employees.FindAsync(id);
            string to = emp.Email;
            String subject = "Regarding The Your Attendence Issue";

            MailMessage mm = new MailMessage();
            mm.To.Add(to);
            mm.Subject = subject;
            mm.From = new MailAddress("bellvantagecom737@gmail.com");
            mm.Body += " Hi " + emp.EmpFName + ",<br>";
            mm.Body += " You have these attendence issues in the folowing days.<br><br>";
            mm.Body += " <html>";
            mm.Body += "<body>";

            foreach (var date in data)
            {
                if (date.Status == "LoggedIn")
                {
                    mm.Body += "<table style='border: 1px solid black'>";
                    mm.Body += "<tr  style='border: 1px solid black'>";
                    mm.Body += "<td  style='border: 1px solid black'><b> Date</b> : " + date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'><b>Issue</b> : In & Out Time Not Marked</td>";
                    mm.Body += "</tr>";
                    mm.Body += "</table><br>";
                }
                if (date.Status == "Not Complete")
                {
                    var info = await _context.MarkAttendence.FindAsync(date.MarkAttendenceId);
                    var timeSpan1 = TimeSpan.FromHours(info.TotalHours);
                    int hh1 = timeSpan1.Hours;
                    int mm1 = timeSpan1.Minutes;
                    String total = hh1 + " h " + mm1 + " min";
                    mm.Body += "<table style='border: 1px solid black'>";
                    mm.Body += "<tr  style='border: 1px solid black'>";
                    mm.Body += "<td  style='border: 1px solid black'> <b>Date </b>: " + date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'> <b>In</b> : " + info.InTime + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'><b> Out</b> : " + info.OutTime + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'> <b>Total Hours</b> : " + total + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'><b>Issue</b> : Work Hours Incomplete</td>";
                    mm.Body += "</tr>";
                    mm.Body += "</table><br>";
                }
                if (date.Status == "Absent")
                {
                    var info = await _context.MarkAttendence.FindAsync(date.MarkAttendenceId);
                    mm.Body += "<table style='border: 1px solid black'>";
                    mm.Body += "<tr  style='border: 1px solid black'>";
                    mm.Body += "<td  style='border: 1px solid black'> Date : " + date.Date.Day + "/" + date.Date.Month + "/" + date.Date.Year + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'> In : " + info.InTime + "</td>";
                    mm.Body += "<td  style='border: 1px solid black'>Issue : Out Time Not marked</td>";
                    mm.Body += "</tr>";
                    mm.Body += "</table>";
                }
                await EmailSent(date.MarkAttendenceId);
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

        public async Task<List<MarkAttendenceModel>> GetTimeSdates(int id)
        {
            var UserEmailOption = new List<MarkAttendenceModel>();
            var AllDates = await _context.MarkAttendence.Where(x => (x.EmpId == id) && (x.Date < DateTime.Now.AddDays(-1)) &&
            (x.EmailSent == false) && ((x.Status == "Not Complete") || (x.Status == "LoggedIn") || (x.Status == "Absent" && x.Type == "Auto"))).ToListAsync();
            if (AllDates?.Any() == true)
            {
                foreach (var dates in AllDates)
                {
                    UserEmailOption.Add(new MarkAttendenceModel()
                    {
                        MarkAttendenceId = dates.MarkAttendenceId,
                        Date = (DateTime)dates.Date,
                        EmpId = id,
                        Status = dates.Status,

                    });
                }
            }
            return UserEmailOption;
        }


        public async Task<bool> EmailSent(int id)
        {

            var emp = await _context.MarkAttendence.FindAsync(id);
            emp.EmailSent = true;

            _context.Entry(emp).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;

        }



    }
}
