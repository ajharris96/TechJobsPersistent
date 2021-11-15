using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob(AddJobViewModel addJobViewModel)
        {
            addJobViewModel.SelectListItem = context.Employers.ToList();
            addJobViewModel.PossibleSkills = context.Skills.ToList();

            return View(addJobViewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                Job job = new Job(addJobViewModel.Name, addJobViewModel.EmployerId);
                context.Jobs.Add(job);
                context.SaveChanges();
                int i = 0;
                List<Job> list = context.Jobs.ToList();
                foreach (Job j in list)
                {
                    if (j == job)
                    {
                        i = j.Id;
                        break;
                    }
                }

                foreach (string s in selectedSkills)
                {
                   
                    JobSkill newSkill = new JobSkill(job.Id, int.Parse(s));
                   
                    context.JobSkills.Add(newSkill);

                    
                }
               
                context.SaveChanges();
                return Redirect("/Home/");
            }

            return View("Add", addJobViewModel);


          
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
