using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace VPMS_Project.Utility
{
    public class EmailSender
    {
        public static void BudgetRemainder()
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("projectvpms@gmail.com", "Vpms12345@"),
                EnableSsl = true,
            };

            smtpClient.Send("projectvpms@gmail.com", "manula.wishvajee@gmail.com", "Bugdet remainder", "Please make the payment for the project");
        }
    }
}
