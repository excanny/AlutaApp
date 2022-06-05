

using AlutaApp.Models;

namespace AlutaApp.ViewModels
{
  
    public class RoleEditx
    {
        public string Role { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Institution> Institutions { get; set; }
        public List<Department> Departments { get; set; }
        public List<Points2Earn> Points { get; set; }
        public List<User> UserAccounts { get; set; }
        public List<Post> Posts { get; set; }
        public List<Document> Documents { get; set; }
        public List<Trivia> Trivias { get; set; }
        public List<TGIFMatch> TGIFMatches { get; set; }
        public List<TimeTable> TimeTables { get; set; }
        public List<CGPA> CGPAs { get; set; }
        public List<DocumentComment> DocumentComments { get; set; }
        public List<Status> Statuses { get; set; }
        public List<Notification> Notifications { get; set; }

        public List<Promotion> Promotions { get; set; }

        public List<BannerAd> BannerAds { get; set; }

        public List<ChatGroup> ChatGroups { get; set; }
    }
    
}
