using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.OrderTransaction.GetAllMeasurements
{
    public class GetAllMeasurementsQueryHandler : IRequestHandler<GetAllMeasurementsQueryRequest, GetAllMeasurementsQueryResponse>
    {
        private readonly IFileReader _fileReader;

        public GetAllMeasurementsQueryHandler(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public async Task<GetAllMeasurementsQueryResponse> Handle(GetAllMeasurementsQueryRequest request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.Path) || string.IsNullOrEmpty(request.OrderName))
            {
                return new GetAllMeasurementsQueryResponse
                {
                    IsSuccess = false,
                    Message = "Siparişin adres dizini ya da ismi bulunamadı."
                };
            }

            var Order = await _fileReader.ReadFile(request.Path,request.OrderName);

            if (Order == null)
            {
                return new GetAllMeasurementsQueryResponse
                {
                    IsSuccess = false,
                    Message = "Sipariş görüntülenirken bir hata oluştu."
                };
            }

            if (Order.SawnPiece.Count() == 0)
            {
                return new GetAllMeasurementsQueryResponse
                {
                    IsSuccess = false,
                    Message = "İlgili siparişte kesilen parça bulunmamaktadır."
                };
            }

            return new GetAllMeasurementsQueryResponse
            {
                IsSuccess = true,
                Message = "Sipariş başarıyla görüntülendi.",
                Orders = Order
            };
        }
    }
}
