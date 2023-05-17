using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebFrontToBack.Areas.Admin.ViewModels
{
    public class CreateRecentWorksVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required, NotMapped]
        public IFormFile Photo { get; set; }
    }
}
