using System;
using System.Net.Http;
using System.Threading.Tasks;
using API_TRON.Exception;
using API_TRON.Model.SmartContract;
using API_TRON.Services.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleBase;

namespace API_TRON.Services
{
    public static class AccountInfoService
    {
        public static async Task<long> GetBalanceAsync(string address, string addressContract)
        {
            var response = await new HttpClient().SendAsync(GetHttpConnectionClientAsync(address, addressContract));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var message = JObject.Parse(responseBody);
            if (message.ToObject<ErrorSmartContractModel>().result.result.ToString() != "True")
            {
                throw new ContractException("Контракт не найден");
            }
            var smartContractInfoModel = message.ToObject<SmartContractInfoModel>();
            var result = long.Parse(smartContractInfoModel.constant_result[0],
                System.Globalization.NumberStyles.HexNumber)/1000000;

            return result;
        }

        private static HttpRequestMessage GetHttpConnectionClientAsync(string address, string contractAddress)
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
                Content = new StringContent(JsonConvert.SerializeObject(new SmartContractRequest()
                {
                    contract_address = contractAddress,
                    parameter = $"0000000000000000000000{addressHex}",
                    function_selector = "balanceOf(address)",
                    owner_address = address,
                    visible = true
                }))
            };
            return request;
        }
    }
}