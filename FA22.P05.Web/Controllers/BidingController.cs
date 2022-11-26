using FA22.P05.Web.Data;
using FA22.P05.Web.Extensions;
using FA22.P05.Web.Features.BidingFeatureFiles;
using FA22.P05.Web.Features.Listings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FA22.P05.Web.Controllers
{
    [Route("api/bids")]
    [ApiController]
    public class BidingController : ControllerBase
    {
        private readonly DbSet<Bids> _bids;
        private readonly DataContext _dataContext;
        private readonly DbSet<Listing> _listings;

        public BidingController(DataContext dataContext)
        {
            _dataContext = dataContext;
            _bids = dataContext.Set<Bids>();
            _listings = dataContext.Set<Listing>();
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public ActionResult<BidsDto> GetBidById(int id)
        {
            var result = _bids.FirstOrDefault(x => x.Id == id);
            if(result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public  ActionResult<BidsDto> CreateBid(BidsDto bidsDto)
        {
            if (IsInvalid(bidsDto)){
                    return BadRequest();
                 }
            var listing = _listings.FirstOrDefault(x => x.Id == bidsDto.ListingId);

            if(listing == null) {
                return BadRequest();
            }
            var bid = new Bids {
                BidAmount = bidsDto.BidAmount,
                ListingId = listing.Id,
                UserId = User.GetCurrentUserId() ?? throw new Exception("missing user id")
            };
            _bids.Add(bid);
           _dataContext.SaveChanges();
            bidsDto.Id = bid.Id;

            return CreatedAtAction(nameof(GetBidById), new { id = bidsDto.Id }, bidsDto);
        }
       
        private static bool IsInvalid(BidsDto dto)
        {
            return ((dto.BidAmount <= 0) || (dto.UserId <= 0 ) || (dto.ListingId <= 0));
        }
        public static IQueryable<BidsDto> GetBidDtos(IQueryable<Bids> bids)
        {
            return bids
                .Select(x => new BidsDto
                {
                    Id = x.Id,
                    BidAmount = x.BidAmount,
                    UserId = x.UserId,
                    ListingId = x.Listing!.Id,
                });
        }
    }
}