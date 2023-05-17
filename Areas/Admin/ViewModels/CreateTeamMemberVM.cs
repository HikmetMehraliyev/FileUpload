using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateTeamMemberVM
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }
        [Required]
        public string Profession { get; set; }
        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
