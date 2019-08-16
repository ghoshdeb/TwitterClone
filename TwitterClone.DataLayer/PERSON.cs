using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.DataLayer
{
    [Table("person")]
    public class PERSON
    {
        [Column(Order =0)]
        [Key]
        [StringLength(25)]
        public string user_id { get; set; }
        [Column(Order = 1)]
        [StringLength(50)]
        [Required]
        public string password { get; set; }
        [StringLength(30)]
        [Column("Name", Order = 2)]
        [Required]
        public string fullName { get; set; }
        [StringLength(50)]
        [Required]
        [Column("Email", Order = 3)]
        
        public string email { get; set; }
        [Column("Joining Date", Order = 4,TypeName ="date")]
        [Required]
        public DateTime joined { get; set; }
        [Column("Active user", Order = 5)]
        [Required]
        public bool active { get; set; }
        public ICollection<TWEET> tweet { get; set; }
        [ForeignKey("user_id")]
        public ICollection<PERSON> following { get; set; }
        [ForeignKey("user_id")]
        [InverseProperty("following")]
        public ICollection<PERSON> followers { get; set; }
    }
}
