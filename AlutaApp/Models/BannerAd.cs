using System;
namespace AlutaApp.Models
{
    public class BannerAd
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string BannerLink { get; set; }

        public string BannerURL { get; set; }

        public int? InstitutionId { get; set; }
        public Institution Institution { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public string Gender { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Cost { get; set; }

        public bool PaidWithPoints { get; set; }

        public int Views { get; set; }

        public DateTime TimeCreated { get; set; }

        public int Status { get; set; }

        public BannerAd()
        {
        }
    }
}
