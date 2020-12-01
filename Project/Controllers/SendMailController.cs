using Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class SendMailController : Controller
    {
        // GET: SendMail
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MailModel _objModelMail, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress("fire31386@gmail.com", "Return Of Firewalls Team");
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                if (upload != null)
                {

                    string filename = Path.GetFileName(upload.FileName);
                    mail.Attachments.Add(new Attachment(upload.InputStream, filename));
                }


                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential

                ("fire31386@gmail.com", "xrdgzhacjsvarnfr");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("ThankYou");
            }
            else
            {
                return View();
            }
        }
    }
}