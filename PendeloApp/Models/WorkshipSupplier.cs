using PendeloApp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PendeloApp.Models
{
    public class WorkshipSupplier
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string CompanyName { get; set; }

        public string Services { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

    }
}
