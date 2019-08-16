using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace TwitterClone.UI.Models
{
    public class PersonVM
    {
        [Required]
        [MaxLengthAttribute(25,ErrorMessage ="Maximum 25 chars allowed")]
        public string Username{ get; set; }
        [DisplayName("Password")]
        [Required]
        [MaxLengthAttribute(50,ErrorMessage = "Maximum 50 chars allowed")]
        public string Pwd { get; set; }
        [Required]
        [MaxLengthAttribute(30, ErrorMessage = "Maximum 30 chars allowed")]
        [DisplayName("Fullname")]
        public string Name { get; set; }
        [Required]
        [MaxLengthAttribute(50, ErrorMessage = "Maximum 50 chars allowed")]
        [EmailAddress(ErrorMessage ="Please provide the valid email")]
        public string Email { get; set; }
        public ICollection<TweetVM> tweet { get; set; }

    }
}