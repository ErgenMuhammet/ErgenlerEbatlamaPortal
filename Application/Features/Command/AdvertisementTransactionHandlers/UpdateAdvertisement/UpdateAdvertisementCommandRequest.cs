using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.UpdateAdvertisement
{
    public class UpdateAdvertisementCommandRequest : IRequest <UpdateAdvertisementCommandResponse>
    {
        [JsonIgnore]
        public string? OwnerId { get; set; }
        [JsonIgnore]
        public string? AdvertisementId { get; set; }
        public string Title { get; set; }
        public string AdvertisementAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string ImgUrl { get; set; }
    }
}
