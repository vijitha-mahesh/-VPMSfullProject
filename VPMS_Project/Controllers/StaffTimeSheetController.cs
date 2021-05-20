using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPMS_Project.Models;
using VPMS_Project.Repository;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VPMS_Project.Controllers
{
    public class StaffTimeSheetController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly TaskRepo _taskRepository = null;
        private readonly Repo2 _Repository2 = null;
        private readonly Repo _Repository = null;



        public StaffTimeSheetController(Repo2 repository2, IWebHostEnvironment webHostEnvironment, TaskRepo taskRepository,Repo repo)
        {
            _taskRepository = taskRepository;
            _Repository2 = repository2;
            _webHostEnvironment = webHostEnvironment;
            _Repository = repo;

        }

        public async Task<IActionResult> TimeSheet(int id, DateTime Start, DateTime End, String Complete, int ID = 0)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            ViewBag.EmpId = Currentuser.EmpId;
            ViewBag.TaskId = ID;

            if (ID != 0)
            {
                await _taskRepository.AddTaskFromList(ID);
            }
            if (Start != DateTime.MinValue && End != DateTime.MinValue)
            {
                await _taskRepository.TImeSheetTaskInsert(id, Start, End);
                await _taskRepository.StatusUpdate(ViewBag.EmpId, Start);
                await _Repository2.AddCost(id, Start, End);

                if (Complete == "Complete")
                {
                    await _taskRepository.CompleteTask(id);
                }
                if (Complete == "NotComplete")
                {
                    await _taskRepository.NotCompleteTask(id);
                }
                return RedirectToAction(nameof(TimeSheet), new {ID = 0 });

            }
            var data = await _taskRepository.AddTaskList(ViewBag.EmpId);
            return View(data);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            bool success = await _taskRepository.CancelAddingTask(id);

            if (success)
            {
                return RedirectToAction(nameof(TimeSheet));
            }
            return RedirectToAction(nameof(TimeSheet));
        }

        [HttpGet]
        public async Task<IActionResult> AddTask(int projectId, String Task, String Des, DateTime S_Date, DateTime E_Date)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int empId = Currentuser.EmpId;
            ViewBag.EmpId = empId;
            ViewBag.proj = new SelectList(await _taskRepository.GetProjects(), "Id", "Name");
            if (projectId == 0)
            {
                return View();
            }
            else
            {
              Boolean s1= await _taskRepository.StaffTaskInsert(empId,projectId,Task, Des,S_Date, E_Date);
              
                if (s1==true)
                {
                    ViewBag.IsSuccess = true;
                    return RedirectToAction(nameof(AddTask));                  
                }
                return View();
          
            }

        }

        [HttpGet]
        public async Task<IActionResult> ViewTimeS(DateTime Date)
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            int id = Currentuser.EmpId;
            if (Date == DateTime.MinValue)
            {
                ViewBag.Empty = true;
                return View();
            }
            else
            {
                ViewBag.Date = Date;

                ViewBag.TotalHours = _taskRepository.GetTotalHours(id, Date);
                var data = await _taskRepository.GetTimeSheet(id, Date);
                if (data != null)
                {
                    ViewBag.Empty = false;
                    return View(data);
                }
                else
                {
                    ViewBag.Empty = true;
                    return View();
                }
            }


        }

        public async Task<IActionResult> CreateDocument()
        {
            string user = User.FindFirst("Index").Value;
            var Currentuser = await _taskRepository.GetCurrentUser(user);
            ViewBag.photo = Currentuser.PhotoURL;
            //Creates a new PDF document
            PdfDocument document = new PdfDocument();
            //Adds page settings
            document.PageSettings.Orientation = PdfPageOrientation.Portrait;
            document.PageSettings.Margins.All = 50;
            //Adds a page to the document
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            //Loads the image as stream
            PdfFont font = new PdfStandardFont(PdfFontFamily.TimesRoman, 24);
            graphics.DrawString("Daily Time Sheet  -  ", font, PdfBrushes.Blue, new PointF(170, 30));
            string Date = DateTime.Now.ToString("MM/dd/yyyy");
            PdfFont font4 = new PdfStandardFont(PdfFontFamily.Helvetica, 18);
            string currentDate = "DATE " + DateTime.Now.ToString("MM/dd/yyyy");
            graphics.DrawString(Date, font4, PdfBrushes.Blue, new PointF(370, 33));

            PdfFont font2 = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
            graphics.DrawString("Ishara Maduhansi", font2, PdfBrushes.Black, new PointF(190, 70));

            PdfFont font3 = new PdfStandardFont(PdfFontFamily.Helvetica, 13);
            graphics.DrawString("Software Engineer", font3, PdfBrushes.Black, new PointF(190, 95));

            FileStream imageStream = new FileStream("wwwroot/images/bell.jpg", FileMode.Open, FileAccess.Read);
            RectangleF bounds = new RectangleF(10, 0, 150, 150);
            PdfImage image = PdfImage.FromStream(imageStream);
            //Draws the image to the PDF page
            page.Graphics.DrawImage(image, bounds);
            PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
            bounds = new RectangleF(0, 160, graphics.ClientSize.Width, 30);
            //Draws a rectangle to place the heading in that region.
            graphics.DrawRectangle(solidBrush, bounds);
            //Creates a font for adding the heading in the page
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
            //Creates a text element to add the invoice number
            PdfTextElement element = new PdfTextElement("Time Sheet ", subHeadingFont);
            element.Brush = PdfBrushes.White;

            //Draws the heading on the page
            PdfLayoutResult result = element.Draw(page, new PointF(10, 165));
            //Measures the width of the text to place it in the correct location
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, 165);
            //Draws the date by using DrawString method
            graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);

            //Creates text elements to add the address and draw it to the page.
            PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.70f);
            PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
            PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);
            ////Draws a line at the bottom of the address
            graphics.DrawLine(linePen, startPoint, endPoint);
            //Creates the datasource for the table
            // Creates a PDF grid
            // Creates the grid cell styles
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Add values to list

            List<object> data = new List<object>();
            object[] ary = new object[2];
            Object row1 = new { task = "Login", Start_Date = Date , End_Date = Date , Effort="2 h 40 Min"};
            Object row2 = new { task = "Time Sheet", Start_Date = Date, End_Date = Date, Effort = "3 h " };
            Object row3 = new { task = "Employee", Start_Date = Date, End_Date = Date, Effort = "2 h 40 Min" };
            Object row4 = new { task = "Leave Part", Start_Date = Date, End_Date = Date, Effort = "4 h 20 Min" };
            Object row5 = new { task = "Attendence Part", Start_Date = Date, End_Date = Date, Effort = "1 h 40 Min" };
            data.Add(row1);
            data.Add(row2);
            data.Add(row3);
            data.Add(row4);
            data.Add(row5);
            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 230));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return fileStreamResult;

        }

      
    }
}
