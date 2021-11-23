using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Models;
using TechJobsPersistent.Data;
using System.Net.Mail;
using System.Net;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class UserController : Controller
    {

        private JobDbContext context;
        private SmtpClient smtpClient;

        public UserController(JobDbContext dbContext)
        {
            context = dbContext;
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("techjobspersistent@gmail.com", "LaunchCode75?"),
                EnableSsl = true,
            };
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            List<string> locations = context.Employers.Select(e=>e.Location).Distinct().ToList();
            ViewBag.locations = locations;


            AddUserViewModel addUserViewModel = new AddUserViewModel();
            return View(addUserViewModel);
        }


        public IActionResult SubmitAddUserForm(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User(addUserViewModel.Name, addUserViewModel.Location, addUserViewModel.Email);
                context.User.Add(user);
                context.SaveChanges();
               
                
                string bodyHTML = user.BuildHtmlString(user.GenerateJobsList(context));

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("techjobspersistent@gmail.com"),
                    Subject = "Welcome, "+user.Name + "!",
                    Body = bodyHTML,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(user.Email);

                smtpClient.Send(mailMessage);

                return View("Index");
            }

            return View("AddUser", addUserViewModel);
        }
    }
}
