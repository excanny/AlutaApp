using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AlutaApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        
        public string Gender { get; set; }

        
        public DateTime DateOfBirth { get; set; }

        
        public int YearOfAdmission { get; set; }

        public string ProfilePhoto { get; set; }

        public string ProfileBanner { get; set; }

        public bool IsVerified { get; set; }

        public int? Referrer { get; set; }

        public int InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public double? GradePoint { get; set; }

        public DateTime TimeRegistered { get; set; }

        public bool Online { get; set; }

        public double Points { get; set; }

        public bool Deleted { get; set; }

        public virtual List<Post> Posts { get; set; }

        public virtual List<HubConnection> HubConnections { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        public virtual List<NewsFeed> Feeds { get; set; }

        public virtual List<UserDevice> Devices { get; set; }

        public virtual List<PointsLog> PointsLogs { get; set; }

        public User()
        {
        }

    }
}
