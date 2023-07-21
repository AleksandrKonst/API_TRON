using Epoche;
using System;
using System.Linq;
using API_TRON.Exception;
using API_TRON.Model.Address;
using API_TRON.Services.Shared;
using Org.BouncyCastle.Crypto.Parameters;
using SimpleBase;

namespace API_TRON.Services
{
    public static class AddressService
    {
        public static AddressModel CreateAddress()
        {
            var keyPair = CryptoService.GenerateKeyPair();
            
            var privateKey = (keyPair.Private as ECPrivateKeyParameters)?.D.ToByteArrayUnsigned();
            var publicKey = (keyPair.Public as ECPublicKeyParameters)?.Q.GetEncoded();
            
            if (publicKey == null || privateKey == null)
            {
                throw new KeyPairException("Ключи равны null");
            }
            
            var hashKeccak = Keccak256.ComputeHash(publicKey.Skip(1).ToArray());
            var address = CryptoService.AddByteToArray(hashKeccak.Skip(12).ToArray(), Convert.ToByte(65));
            var hashFirst = CryptoService.GetHashSha256(address);
            var hashSecond = CryptoService.GetHashSha256(hashFirst);
            var addressWithCheckSum = CryptoService.AddLastBytesToArray(address, hashSecond.Take(4).ToArray());
            var addressBase58 = Base58.Bitcoin.Encode(addressWithCheckSum);
            
            return new AddressModel() {
                publicKey = CryptoService.ToHex(publicKey),
                privateKey = CryptoService.ToHex(privateKey),
                addressBase58 = addressBase58,
                address = CryptoService.ToHex(address)
            };
        }
        
        public static void WriteAccountInfo(AddressModel addressModel)
        {
            Console.WriteLine("PRIVATE_KEY: " + addressModel.privateKey);
            Console.WriteLine("PUBLIC_KEY: " + addressModel.publicKey);
            Console.WriteLine("ADDRESS_BASE58: " + addressModel.addressBase58);
            Console.WriteLine("ADDRESS: " + addressModel.address);
        }
    }
}