using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestingCoreIdentity.Models
{
    public class Post
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }
        public ApplicationUser  User { get; set; }


        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(5000)]
        public string Body { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Author Name")]
        [StringLength(150)]
        public string AuthorName { get; set; }

        [ScaffoldColumn(false)]
        public int Readers { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }
}
