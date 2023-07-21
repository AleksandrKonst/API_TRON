using System;
using System.Net.Http;
using API_TRON.Services.Shared;
using SimpleBase;

namespace API_TRON.Services
{
    public static class AccountInfoService
    {
        public static HttpRequestMessage GetHttpConnectionClientAsync(string address, string contractAddress)
        {
            var addressHex = CryptoService.ToHex(Base58.Bitcoin.Decode(address).ToArray());
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://api.nileex.io/wallet/triggersmartcontract"),
                Headers =
                {
                    { "accept", "application/json" },
                },
                Content = new StringContent($"{{\n \"contract_address\": \"{contractAddress}\",\n \"parameter\": \"0000000000000000000000{addressHex}\",\n " +
                                            $"\"function_selector\": \"balanceOf(address)\",\n \"owner_address\": \"{address}\",\n \"visible\": true\n }}")
            };
            return request;
        }
    }
}