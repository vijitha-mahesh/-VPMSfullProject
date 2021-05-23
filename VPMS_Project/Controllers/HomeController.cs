using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VPMS_Project.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using VPMS_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace VPMS_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Repo _repo = null;
        private readonly Repo2 _repo2 = null;
        private readonly Repo3 _repo3 = null;
        private readonly Repo4 _repo4 = null;
        private EmpStoreContext _ctx = null;

        public HomeController(ILogger<HomeController> logger, Repo repo, Repo2 repo2, Repo3 repo3, Repo4 repo4, EmpStoreContext ctx)
        {
            _logger = logger;
            _repo = repo;
            _repo2 = repo2;
            _repo3 = repo3;
            _repo4 = repo4;
            _ctx = ctx;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index2()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Manager()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProjects(bool success = false, bool search = false)
        {
            ViewBag.Success = success;
            ViewBag.search = search;
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["stages"] = await _repo.ProjectStatus3();
            var data = await _repo.GetProjects();
            return View(data);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProjectsByType(string type, bool success = false)
        {
            ViewBag.Success = success;
            ViewBag.type = type;
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["stages"] = await _repo.ProjectStatus3();
            var data = await _repo.GetProjectsByType(type);
            return View(data);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProjectsByState(string state, bool success = false)
        {
            ViewBag.Success = success;
            ViewBag.state = state;
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["stages"] = await _repo.ProjectStatus3();
            var data = await _repo.GetProjectsByState(state);
            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetCurrentProjects(bool success = false)
        {
            ViewBag.Success = success;
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["stages"] = await _repo.ProjectStatus3();
            var data = await _repo.GetCurrentProjects();
            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetClosedProjects(bool success = false)
        {
            ViewBag.Success = success;
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["stages"] = await _repo.ProjectStatus3();
            var data = await _repo.GetClosedProjects();
            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var data = await _repo.GetProjectById(id);


            if (data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays <= 0)
            {
                ViewBag.TimePresentage = 0;
                ViewBag.TimeDays = 0;
            }
            else if (data.StartDate.Date > DateTime.Now.Date)
            {
                ViewBag.TimePresentage = 100;
                ViewBag.TimeDays = data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
            }
            else
            {
                ViewBag.TimePresentage = Math.Round((data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays) / (data.DeliveryDate.Date.Subtract(data.StartDate).TotalDays) * 100);
                ViewBag.TimeDays = data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
            }

            if (data.EstimetedBudget == 0)
            {
                ViewBag.BudgetRemaining = 0;
                ViewBag.BudgetPresentage = 0;
            }
            else
            {
                ViewBag.BudgetRemaining = data.EstimetedBudget - data.Cost;
                ViewBag.BudgetPresentage = Math.Round(((data.EstimetedBudget - data.Cost) / data.EstimetedBudget) * 100);
            }

            if (data.ContractValue == null || data.ContractValue == 0)
            {
                ViewBag.ProfitMargin = 0;
                ViewBag.Profit = 0;
                ViewBag.ProfitMarginTarget = 0;
            }
            else
            {
                ViewBag.ProfitMargin = Math.Round((double)((data.ContractValue - data.Cost) / data.ContractValue * 100));
                ViewBag.Profit = data.ContractValue - data.Cost;
                ViewBag.ProfitMarginTarget = Math.Round((double)((data.ContractValue - data.EstimetedBudget) / data.ContractValue * 100));
            }
            var totalTasks = await _repo2.TaskCount(data.Id);
            ViewBag.TotalTasks = totalTasks;
            if (totalTasks != 0)
            {
                ViewBag.Completion = Math.Round((double)((data.FinalizedTasks / totalTasks) * 100));
            }
            else
            {
                ViewBag.Completion = 0;
            }


            ViewBag.pm = await _repo2.GetEmployeeNameById(data.ProjectManagerId);
            if (data.CustomerId != null)
            {
                ViewBag.cus = await _repo3.GetCustomerNameById(data.CustomerId);
            }
            else
            {
                ViewBag.cus = "none";
            }

            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProject(bool Success = false, int Id = 0)
        {
            ViewBag.Success = Success;
            ViewBag.id = Id;
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            ViewBag.Customer = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            var data = await _repo.GetProjectById(Id);
            return View(data);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(Project project)
        {
            if (ModelState.IsValid)
            {
                bool success = await _repo.EditProject(project);
                if (success == true)
                {
                    return RedirectToAction(nameof(EditProject), new { Success = true });
                }
            }
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            ViewBag.Customer = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProject(bool isSuccess = false, bool currentContext = false, bool invalid = false, int Id = 0)
        {
            ViewBag.isSuccess = isSuccess;
            ViewBag.context = currentContext;
            ViewBag.invalid = invalid;
            ViewBag.id = Id;
            ViewData["projects"] = await _repo.GetProjects2();
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            ViewBag.Customer = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddProject(Project project)
        {

            if ((project.StartDate < DateTime.Now) || (project.DeliveryDate < DateTime.Now))
            {
                return RedirectToAction(nameof(AddProject), new { currentContext = true });
            }
            if (project.StartDate >= project.DeliveryDate)
            {
                return RedirectToAction(nameof(AddProject), new { invalid = true });
            }

            if (ModelState.IsValid)
            {
                int id = await _repo.AddProject(project);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddProject), new { isSuccess = true, Id = id });
                }
            }
            ViewBag.ProjectManager = new SelectList(await _repo2.GetEmployeeByTitle("project manager"), "Id", "Name");
            ViewBag.Customer = new SelectList(await _repo3.GetCustomers(), "Id", "Name");
            ViewData["projects"] = await _repo.GetProjects2();
            return View();
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            bool add = false;
            add = await _repo.AddToDeleted(id);
            if (add == true)
            {
                bool success = await _repo.DeleteProject(id);
                return RedirectToAction(nameof(GetProjects), new { success = add });
            }

            return null;

        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> SearchProject(String Name)
        {
            if (Name != null)
            {
                var project = await _repo.SearchProject(Name);
                if (project?.Any() == true)
                {
                    ViewData["types"] = await _repo.ProjectStatus();
                    ViewData["states"] = await _repo.ProjectStatus2();
                    ViewData["stages"] = await _repo.ProjectStatus3();
                    return View(project);
                }
                return RedirectToAction(nameof(GetProjects), new { search = true });
            }
            return RedirectToAction(nameof(GetProjects));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ViewProgress(int id)
        {
            var data = await _repo.GetProjectById(id);
            ViewBag.ProfitMargin = Math.Round((double)((data.ContractValue - data.Cost) / data.ContractValue * 100));
            ViewBag.Profit = data.ContractValue - data.Cost;
            ViewBag.ProfitMarginTarget = Math.Round((double)((data.ContractValue - data.EstimetedBudget) / data.ContractValue * 100));
            if (data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays <= 0)
            {
                ViewBag.TimePresentage = 0;
                ViewBag.TimeDays = 0;
            }
            else
            {
                ViewBag.TimePresentage = Math.Round((data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays) / (data.DeliveryDate.Date.Subtract(data.StartDate).TotalDays) * 100);
                ViewBag.TimeDays = data.DeliveryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
            }

            if (data.EstimetedBudget == 0)
            {
                ViewBag.BudgetRemaining = 0;
                ViewBag.BudgetPresentage = 0;
            }
            else
            {
                ViewBag.BudgetRemaining = data.EstimetedBudget - data.Cost;
                ViewBag.BudgetPresentage = Math.Round(((data.EstimetedBudget - data.Cost) / data.EstimetedBudget) * 100);
            }

            return View(data);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ProjectPlanAsync(int id)
        {
            var data = await _repo.GetProjectById(id);
            return View(data);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> ProjectStatus()
        {
            ViewData["types"] = await _repo.ProjectStatus();
            ViewData["states"] = await _repo.ProjectStatus2();
            ViewData["states1"] = await _repo.ProjectStatus4();
            ViewData["states2"] = await _repo.ProjectStatus5();
            ViewData["stages"] = await _repo.ProjectStatus3();
            ViewData["month"] = await _repo.ProjectStatusByMonth();
            ViewData["month2"] = await _repo.ProjectStatusByMonth2();
            return View();
        }
        [Authorize(Roles = "admin")]
        public IActionResult ProjectStatus2()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult ProjectStatusByMonth()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Chart1()
        {

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Chart2()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Chart3()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult ProjectType()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult ProjectState()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult ProjectStage()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult ViewPlans()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UserDashboard()
        {
            ViewData["projects1"] = await _repo4.ProjectCategories1();
            ViewData["projects2"] = await _repo4.ProjectCategories2();
            ViewData["Tasks"] = await _repo4.TaskCategories();
            ViewData["Employees"] = await _repo4.GetDashboardEmps();
            ViewData["Tasks2"] = await _repo4.TaskCategories2();

            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult CreateDocument()
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            //Load the image as stream.
            FileStream imageStream = new FileStream("wwwroot/images/Melstacorp.jpg", FileMode.Open, FileAccess.Read);
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draw the image
            graphics.DrawImage(image, 0, 0);

            //Draw the text
            graphics.DrawString("Project Overview", font, PdfBrushes.Black, new PointF(0, 0));



            /*  //Creates the datasource for the table
              System.Data.DataTable invoiceDetails = _repo.ProjectStatus();
              //Creates a PDF grid
              PdfGrid grid = new PdfGrid();
              //Adds the data source
              grid.DataSource = invoiceDetails;
              //Creates the grid cell styles
              PdfGridCellStyle cellStyle = new PdfGridCellStyle();
              cellStyle.Borders.All = PdfPens.White;
              PdfGridRow header = grid.Headers[0];
              //Creates the header style
              PdfGridCellStyle headerStyle = new PdfGridCellStyle();
              headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
              headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
              headerStyle.TextBrush = PdfBrushes.White;
              headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

              //Adds cell customizations
              for (int i = 0; i < header.Cells.Count; i++)
              {
                  if (i == 0 || i == 1)
                      header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                  else
                      header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
              }

              //Applies the header style
              header.ApplyStyle(headerStyle);
              cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
              cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
              cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));
              //Creates the layout format for grid
              PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
              // Creates layout format settings to allow the table pagination
              layoutFormat.Layout = PdfLayoutType.Paginate;
              //Draws the grid to the PDF page.
              PdfGridLayoutResult gridResult = grid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

              */


            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            fileStreamResult.FileDownloadName = "Sample.pdf";

            return fileStreamResult;



        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //+++++++++++++++++Live chat Home++++++++++++++++++++++


        [Authorize(Roles = "admin")]
        public IActionResult BrowseRooms()
        {
            var chats = _ctx.Chats.Include(x => x.Users).Where(x => !x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)).Where(x => x.Type != ChatType.Private)
            .ToList();

            return View(chats);
        }
        [Authorize(Roles = "admin")]
        public IActionResult ChatIndex() => View();

        public IActionResult Find()
        {
            var users = _ctx.Users.Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .ToList();

            return View(users);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Private()
        {
            var chats = _ctx.Chats
            .Include(x => x.Users)
            .ThenInclude(x => x.User)
            .Where(x => x.Type == ChatType.Private
            && x.Users
                .Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();

            return View(chats);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var chat = new Chat
            {
                Name = "Private",
                Type = ChatType.Private
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chat.Id });
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
            return View(chat);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId });
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.admin
            });

            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = UserRole.admin
            };

            _ctx.ChatUsers.Add(chatUser);
            await _ctx.SaveChangesAsync();

            return RedirectToAction("Chat", "Home", new { id = id });
        }
    }
}



