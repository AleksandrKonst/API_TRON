using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API_TRON.Model.Transaction;
using API_TRON.Services.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using SimpleBase;

namespace API_TRON.Services
{
    public static class TransactionService
    {
        public static async Task CreateTransactionAsync(string addressFrom, string addressTo, string priKey, int amount)
        {
            var response = await new HttpClient().SendAsync(GetTransactionHttpConnectionClientAsync(addressFrom, addressTo, amount));
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var message = JObject.Parse(responseBody);
            var getTransactionModel = message.ToObject<GetTransactionModel>();
            
            Console.WriteLine("Тестовая транзакция:\n" + message);
            
            var sigBytes = SignTransaction2Byte(getTransactionModel, priKey);
            
            Console.WriteLine("Тестовая подпись: " + CryptoService.ToHex(sigBytes));
        }
        
        private static HttpRequestMessage GetTransactionHttpConnectionClientAsync(string addressFrom, string addressTo, int amount)
        {
            var addressFromHex = CryptoService.ToHex(Base58.Bitcoin.Decode(addressFrom).ToArray().Take(21).ToArray());
            var addressToHex = CryptoService.ToHex(Base58.Bitcoin.Decode(addressTo).ToArray().Take(21).ToArray());
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://api.nileex.io/wallet/createtransaction"),
                Headers =
                {
                    { "accept", "application/json" },
                },
                Content = new StringContent(JsonConvert.SerializeObject(new CreateTransactionRequest()
                {
                    owner_address = addressFromHex,
                    to_address = addressToHex,
                    amount = amount
                }))
            };
            return request;
        }

        private static byte[] SignTransaction2Byte(GetTransactionModel transaction, string priKey)
        {
            byte[] hash = CryptoService.GetHashSha256(CryptoService.ObjectToByteArray(transaction.raw_data.GetHashCode()));
            X9ECParameters ecParams = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domainParams = new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());
            ECPrivateKeyParameters privateKeyParameters = new ECPrivateKeyParameters(new BigInteger(priKey, 16), domainParams);
            ECDsaSigner signer = new ECDsaSigner();
            signer.Init(true, privateKeyParameters);
 
            BigInteger[] sigs = signer.GenerateSignature(hash);

            byte[] sigBytes = CryptoService.AddLastBytesToArray(sigs[0].ToByteArray(), sigs[1].ToByteArray());

            return sigBytes;
        }
    }
}