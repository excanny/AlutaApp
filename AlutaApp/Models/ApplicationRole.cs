using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlutaApp.Models
{

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole(string name) : base(name)
        {
        }
        public string? Description { get; set; }
    }
}
