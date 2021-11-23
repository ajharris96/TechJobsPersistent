using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Models;
using TechJobsPersistent.Data;
using System.Net.Mail;
using System.Net;

namespace TechJobsPersistent.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        



        public User()
        {
        }

        public User(string name, string location, string email)
        {
            Name = name;
            Location = location;
            Email = email;
        }

        public List<Job> GenerateJobsList(JobDbContext context)
        {
            List<Job> jobs = context.Jobs.ToList();
            List<Employer> employers = context.Employers.ToList();
            List<Job> userJobs = new List<Job>();

            foreach (Employer e in employers)
            {
                if (e.Location.ToLower() == Location.ToLower())
                {
                    foreach (Job job in jobs)
                    {
                        if (e.Id == job.EmployerId)
                        {
                            userJobs.Add(job);
                        }
                    }
                }
            }
            return userJobs;
        }

        public string BuildHtmlString(List<Job> userJobs)
        {
            string bodyHTML = "<h2>Here are the best job opportunities available to you!</h1></br><ul>";

            for (int i = 0; i < userJobs.Count; i++)
            {
                bodyHTML += "<li>" + userJobs[i].Name + ", " + userJobs[i].Employer.Name + ", " + userJobs[i].Employer.Location + "</li></br>";
            }

            bodyHTML += "</ul>";

            return bodyHTML;
        }

        public void SendUpdate(Job job, SmtpClient smtpClient)
        {
            string bodyHTML = "<h3>A new job for you was just posted!</h3>";
            bodyHTML += "<p>" + job.Name + ", " + job.Employer.Name + ", " + job.Employer.Location + "</p>";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("techjobspersistent@gmail.com"),
                Subject = "New job posting!",
                Body = bodyHTML,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(Email);

            smtpClient.Send(mailMessage);
        }




    }
}
