using System;
using System.Net.Http;
using System.Threading.Tasks;
using API_TRON.Exception;
using API_TRON.Model.Address;
using API_TRON.Model.SmartContract;
using API_TRON.Model.Transaction;
using API_TRON.Services;
using API_TRON.Services.Shared;
using Newtonsoft.Json.Linq;

namespace API_TRON
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Menu: \n1. CreateAccount \n2. GetBalance \n3. GetHistoryOperations \n4. CreateTransaction");
                switch (Console.ReadLine())
                {
                    case "CreateAccount":
                        CreateAccount();
                        break;
                    case "GetBalance":
                        await GetBalance();
                        break;
                    case "GetHistoryOperations":
                        await GetHistoryOperationsAsync();
                        break;
                    case "CreateTransaction":
                        await GetHistoryOperationsAsync();
                        break;
                    default:
                        return;
                }
            }
        }

        private static void CreateAccount()
        {
            AddressModel addressModel;
            
            addressModel = AddressService.CreateAddress();
            AddressService.WriteAccountInfo(addressModel);
        }

        private static async Task GetBalance()
        {
            Console.WriteLine("Address. Example: TRQfYEkdqvWft5pb3PGzERX6Woh5v7syAV");
            var address = CheckService.CheckAddress(Console.ReadLine());
            Console.WriteLine("AddressContract. Example: TXYZopYRdj2D9XRtbG411XZZ3kM5VkAeBf");
            var addressContract = CheckService.CheckAddress(Console.ReadLine());
            
            var response = await new HttpClient().SendAsync(AccountInfoService.GetHttpConnectionClientAsync(address, addressContract));
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
            Console.WriteLine(result + " USDT");
        }
        
        private static async Task GetHistoryOperationsAsync()
        {
            TransactionInfoModel transactionInfoModel;
            
            Console.WriteLine("Address. Example: TNPns1Wa3NZYozYKTJvsEshk6FS4opWgnf");
            var address = CheckService.CheckAddress(Console.ReadLine());
            Console.WriteLine("DateBegin");
            var dateBegin = CheckService.CheckDataTime(Console.ReadLine());
            Console.WriteLine("DateEnd");
            var dateEnd = CheckService.CheckDataTime(Console.ReadLine());

            var response = await new HttpClient().SendAsync(GetHistoryOperationServices.GetHttpConnectionClientAsync(address));
            var messageResponse = response.Content.ReadAsStringAsync();
            var message = JObject.Parse(messageResponse.Result);
            if (message.ToObject<ErrorTransactionModel>().statusCode == 400)
            {
                throw new TransactionException("Аккаунт не найден");
            }
            transactionInfoModel = message.ToObject<TransactionInfoModel>();
 
            Console.WriteLine("Transaction");
            foreach (var transaction in transactionInfoModel.data)
            {
                var normalDataTime = GetHistoryOperationServices.ParseToNormalDataTimeType(transaction);
                var resultCompareDataBegin = DateTime.Compare(normalDataTime, dateBegin);
                var resultCompareDataEnd = DateTime.Compare(normalDataTime, dateEnd);
                if (resultCompareDataBegin >= 0 && resultCompareDataEnd <= 0)
                {
                    GetHistoryOperationServices.WriteTransactionInfo(transaction);
                }
                else
                {
                    throw new TransactionException("Транзакций не найдено");
                }
            }
        }
    }
}