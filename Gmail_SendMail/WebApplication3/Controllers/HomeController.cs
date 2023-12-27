using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public  ActionResult SendMail(string MyMail) 
        {
            // Specify the file to be attached and sent
            string filePath = @"C:\path\to\attachment.txt";

         
            // Create a message and set up recipients
            MailMessage messageWithAttachment = new MailMessage("寄件人信箱", MyMail, "Important Report", "Please find the attached file.");

            // Create the file attachment
            Attachment attachment = new Attachment(filePath, System.Net.Mime.MediaTypeNames.Application.Octet);

            // Add time stamp information for the file
            ContentDisposition disposition = attachment.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);

            // Add the file attachment to the email message
            messageWithAttachment.Attachments.Add(attachment);

            // Send the message
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential("寄件人信箱", "jlqoqycclhnnuxim");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(messageWithAttachment);
               // Console.WriteLine("Email with attachment sent successfully!");
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Exception caught: {ex}");
            }

            // Dispose resources
            messageWithAttachment.Dispose();
            smtpClient.Dispose();
            return RedirectToAction("Index");
        }
    }
}