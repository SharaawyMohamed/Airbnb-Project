using Microsoft.AspNetCore.Http;
namespace Airbnb.Domain.DataTransferObjects.Property
{
    public class UpdatePropertyDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? NightPrice { get; set; }
        public string? PlaceType { get; set; }
        public IEnumerable<string>? Categories { get; set; }
        public IEnumerable<string>? RoomServices { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }

    }
}
