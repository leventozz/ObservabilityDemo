using Common.Shared.DTOs;

namespace Stock.API.PaymentServices
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool isSuccess, string? failMessage)> PaymentCreateProcess(PaymentCreateRequestDto processRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentCreateRequestDto>("api/Payment/Create", processRequestDto);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseDto<PaymentCreateResponseDto>>();

            return response.IsSuccessStatusCode ? (true, null) : (false, responseContent!.Errors!.First());
        }
    }
}
