using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.DataLayer
{
   [Table("tweet")]
    public class TWEET
    {
        [Column(Order = 0)]
        [Key]
        public int tweet_id { get; set; }
        [Column(Order =1)]
        [Required]
        [StringLength(25)]
        public string user_id { get; set; }
        [Required]
        [Column("TweetMessage", Order = 2)]
        [StringLength(140)]
        public string message { get; set; }
        [Required]
        [Column("CreatedDate", Order = 3, TypeName = "date")]
        public DateTime created { get; set; }
        [ForeignKey("user_id")]
        public PERSON person { get; set; }
    }
}
