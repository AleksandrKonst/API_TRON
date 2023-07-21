using System;
using System.Threading.Tasks;
using API_TRON.Exception;
using API_TRON.Model.Address;
using API_TRON.Model.Transaction;
using API_TRON.Services;
using API_TRON.Services.Shared;

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
                        await GetBalanceAsync();
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

        private static async Task GetBalanceAsync()
        {
            Console.WriteLine("Address. Example: TRQfYEkdqvWft5pb3PGzERX6Woh5v7syAV");
            var address = CheckService.CheckAddress(Console.ReadLine());
            Console.WriteLine("AddressContract. Example: TXYZopYRdj2D9XRtbG411XZZ3kM5VkAeBf");
            var addressContract = CheckService.CheckAddress(Console.ReadLine());
            var result = await AccountInfoService.GetBalanceAsync(address, addressContract);
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

            transactionInfoModel = await GetHistoryOperationServices.GetHistoryOperationsAsync(address);
 
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