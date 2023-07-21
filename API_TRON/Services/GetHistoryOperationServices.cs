using System;
using System.Net.Http;
using API_TRON.Model.Transaction;

namespace API_TRON.Services
{
    public static class GetHistoryOperationServices
    {
        public static HttpRequestMessage GetHttpConnectionClientAsync(string address)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://nile.trongrid.io//v1/accounts/{address}/transactions/trc20"),
                Headers =
                {
                    { "accept", "application/json" },
                }
            };
            return request;
        }
        
        public static DateTime ParseToNormalDataTimeType(Data transaction)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds(int.Parse(transaction.block_timestamp.Remove(9, 3)));
            return date;
        }

        public static void WriteTransactionInfo(Data oneTransaction)
        {
            Console.WriteLine("ID: " + oneTransaction.transaction_id);
            Console.WriteLine("DATE: " + ParseToNormalDataTimeType(oneTransaction));
            Console.WriteLine("FROM: " + oneTransaction.from);
            Console.WriteLine("TO: " + oneTransaction.to);
            Console.WriteLine("ADDRESS: " + oneTransaction.token_info.address);
            Console.WriteLine("DECIMALS: " + oneTransaction.token_info.decimals);
            Console.WriteLine("NAME: " + oneTransaction.token_info.name);
            Console.WriteLine("SYMBOL: " + oneTransaction.token_info.symbol);
            Console.WriteLine("TYPE: " + oneTransaction.type);
            Console.WriteLine("VALUE: " + oneTransaction.value.Remove(oneTransaction.value.Length - 6, 6) + " \n");
        }
    }
}