using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFrontToBack.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required]
        public string ImagePath { get; set; }
    }
}
