using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TwitterClone.UI.Models
{
    public class TweetVM
    {             
        [StringLength(140)]
        public string message { get; set; }
        public DateTime created { get; set; }
    }
}