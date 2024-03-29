using PendeloApp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PendeloApp.Models
{
    public class KmPerDay
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; } 

        public virtual ApplicationUser User { get; set; }

        [DataType(DataType.Date)]
        public DateTime DriveDate { get; set; }

        public double Kilometers { get; set; }
    }
}
