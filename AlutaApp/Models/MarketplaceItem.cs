using System;
namespace AlutaApp.Models
{
	public class MarketplaceItem
	{
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? InstitutionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int Views { get; set; }

        public int Status { get; set; }

        public bool Sold { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeModified { get; set; }

        public List<MarketPlaceItemImage> Images { get; set; }

        public List<MarketplaceItemComment> Comments { get; set; }

        public MarketplaceItem()
		{
		}
	}
}

