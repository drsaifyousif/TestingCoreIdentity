using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TestingCoreIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [StringLength (150)]
        public string Address { get; set; }


        [DataType(DataType.Date )]
        [Required]
        public DateTime DateofBirth { get; set; }


        [Url]
        public string Url { get; set; }

    }
}
