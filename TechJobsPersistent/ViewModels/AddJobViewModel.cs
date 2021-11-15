using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;
using TechJobsPersistent.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TechJobsPersistent.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        
        public int EmployerId { get; set; }

        public List<Employer> SelectListItem { get; set; }

        public List<Skill> Skills { get; set; }

        public List<Skill> PossibleSkills { get; set; }

        public AddJobViewModel()
        {
            
        }

        
    }
}
