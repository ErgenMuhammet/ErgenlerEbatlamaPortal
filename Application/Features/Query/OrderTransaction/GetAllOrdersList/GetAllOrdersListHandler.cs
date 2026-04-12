using Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.OrderTransaction.GetAllOrdersList
{
    public class GetAllOrdersListHandler : IRequestHandler<GetAllOrdersListRequest, GetAllOrdersListResponse>
    {
        private readonly IFileReader _fileReader;
        public GetAllOrdersListHandler(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }
        public async Task<GetAllOrdersListResponse> Handle(GetAllOrdersListRequest request, CancellationToken cancellationToken)
        {
            var List = await _fileReader.ReadFile();

            if (List.Count == 0 || List == null)
            {
                return new GetAllOrdersListResponse
                {
                    FileDetails = List,
                    IsSuccess = true,
                    Message = "İlgili klasörde görüntülenecek bir sipariş bulunmamaktadır."
                };
            }
            if (List.Count >= 0)
            {
                return new GetAllOrdersListResponse
                {
                    FileDetails = List,
                    IsSuccess = true,
                    Message = "Siparişler Listesi getirildi."
                };
            }
            return new GetAllOrdersListResponse
            {
                FileDetails = null,
                IsSuccess = false,
                Message = "Siparişler Listesi getirilirken bir hata ile karşılaşıldı."

            };
        }
    }
}
