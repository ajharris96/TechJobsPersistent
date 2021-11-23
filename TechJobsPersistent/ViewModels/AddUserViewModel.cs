using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddUserViewModel
    {

        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [EmailAddress(ErrorMessage = "Email must be valid")]
        [Required(ErrorMessage ="Email is required.")]
        public string Email { get; set; }


        public AddUserViewModel()
        {
        }
    }
}
