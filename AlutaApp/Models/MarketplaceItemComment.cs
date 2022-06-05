using System;
namespace AlutaApp.Models
{
	public class MarketplaceItemComment
	{
        public int Id { get; set; }

        public int MarketplaceItemId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Comment { get; set; }

        public DateTime TimeCreated { get; set; }

        public int? MarketplaceItemCommentId { get; set; }
        public List<MarketplaceItemComment> Replies { get; set; }

        public MarketplaceItemComment()
		{
		}
	}
}

