using Common.Shared.DTOs;
using Stock.API.PaymentServices;
using System.Net;

namespace Stock.API
{
    public class StockService
    {
        private readonly PaymentService _paymentService;

        public StockService(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        private Dictionary<int, int> GetProductStockList()
        {
            Dictionary<int, int> productStockList = new();
            productStockList.Add(1, 10);
            productStockList.Add(5, 50);
            productStockList.Add(50, 500);
            return productStockList;
        }
        public async Task<ResponseDto<StockCheckAndPaymentProcessResponsetDto>> CheckAndPaymentProcessAsync(StockCheckAndPaymentProcessRequestDto requestDto)
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
                return ResponseDto<StockCheckAndPaymentProcessResponsetDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), "Insufficient stock!");
            }

            var (isSuccess, failMessage) = await _paymentService.PaymentCreateProcessAsync(new PaymentCreateRequestDto()
            {
                OrderCode = requestDto.OrderCode,
                TotalPrice = (requestDto.OrderItems.Sum(x => x.UnitPrice))
            });

            if(!isSuccess)
                return ResponseDto<StockCheckAndPaymentProcessResponsetDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(),
                failMessage!);

            return ResponseDto<StockCheckAndPaymentProcessResponsetDto>.Success(HttpStatusCode.OK.GetHashCode(),
                new StockCheckAndPaymentProcessResponsetDto() { Description = "Process completed" });
        }
    }
}
