using System.ComponentModel.DataAnnotations;

namespace AlutaApp.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
