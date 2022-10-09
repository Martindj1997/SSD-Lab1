using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab1.Models
{
    public class Team
    {
        [Key, Required, Display(Name = "ID")]
        public int Id { get; set; }
        [Required, Display(Name = "Team Name")]
        public string TeamName { get; set; }
        [Required, Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Established Date")]
        public DateTime EstablishedDate { get; set; }
    }
}
