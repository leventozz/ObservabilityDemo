using Common.Shared.DTOs;
using System.Runtime.InteropServices;

namespace Order.API.StockServices
{
    public class StockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool isSuccess, string? failMessage)> StockCheckAndPaymentStartAsync(StockCheckAndPaymentProcessRequestDto processRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync<StockCheckAndPaymentProcessRequestDto>("api/Stock/StockCheckAndPaymentStart",processRequestDto);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseDto<StockCheckAndPaymentProcessResponsetDto>>();

            return response.IsSuccessStatusCode ? (true, null) : (false, responseContent!.Errors!.First());
        }
    }
}
