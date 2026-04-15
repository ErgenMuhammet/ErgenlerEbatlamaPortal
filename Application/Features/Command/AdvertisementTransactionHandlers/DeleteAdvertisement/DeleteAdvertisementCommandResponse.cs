using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.AdvertisementTransactionHandlers.DeleteAdvertisement
{
    public class DeleteAdvertisementCommandResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
    }
}
