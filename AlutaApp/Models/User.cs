namespace AlutaApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    namespace AlutaApp.Models
    {
        public class User : IdentityUser<int>
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            public string Gender { get; set; }
             
            [Required]
            public DateTime DateOfBirth { get; set; }

            public DateTime TimeRegistered { get; set; }

            [Required]
            public int YearOfAdmission { get; set; }

            public string? ProfilePhoto { get; set; }
            public string Biography { get; set; }
            public string UserType { get; set; }
            public string UserStatus { get; set; }
            public bool IsBanned { get; set; }

            public bool IsVerified { get; set; }

            public int Referrer { get; set; }

            public int? InstitutionId { get; set; }
            public virtual Institution? Institution { get; set; }

            public int? DepartmentId { get; set; }
            public virtual Department? Department { get; set; }

            public double GradePoint { get; set; }

            public bool Online { get; set; }

            public virtual List<Post>? Posts { get; set; }

            public virtual List<HubConnection>? HubConnections { get; set; }

            public virtual List<Notification>? Notifications { get; set; }

            public User()
            {
            }

        }
    }

}
