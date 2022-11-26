﻿using FA22.P05.Web.Features.Authorization;
using FA22.P05.Web.Features.BidingFeatureFiles;
using FA22.P05.Web.Features.ItemListings;

namespace FA22.P05.Web.Features.Listings;

public class Listing
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset StartUtc { get; set; }

    public DateTimeOffset EndUtc { get; set; }

    public int UserId { get; set; }
    public  User? User { get; set; }
    public ICollection<ItemListing>? ItemsForSale { get; set; }
    public ICollection<Bids>? ListingBids { get; set; }
    public int? OwnerId { get; internal set; }
}