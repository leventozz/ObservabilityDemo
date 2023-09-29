using Common.Shared.DTOs;
using System.Net;

namespace Stock.API
{
    public class StockService
    {
        private Dictionary<int, int> GetProductStockList()
        {
            Dictionary<int, int> productStockList = new();
            productStockList.Add(1, 10);
            productStockList.Add(5, 50);
            productStockList.Add(50, 500);
            return productStockList;
        }
        public ResponseDto<StockCheckAndPaymentProcessResponsetDto> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto requestDto)
        {
            var productStockList = GetProductStockList();
            var stockStatus = new List<(int productId, bool hasStockExist)>();

            foreach (var orderItem in requestDto.OrderItems)
            {
                var hasExistStock = productStockList.Any(x => x.Key == orderItem.ProductId
                && x.Value >= orderItem.Count);

                stockStatus.Add((orderItem.ProductId, hasExistStock));
            }
            if (stockStatus.Any(x => x.hasStockExist == false))
            {
                return ResponseDto<StockCheckAndPaymentProcessResponsetDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), "There is no stock!");
            }

            return ResponseDto<StockCheckAndPaymentProcessResponsetDto>.Success(HttpStatusCode.OK.GetHashCode(), 
                new StockCheckAndPaymentProcessResponsetDto() { Description = "Process completed, stock decreased" });

            //Do payment things
        }
    }
}
