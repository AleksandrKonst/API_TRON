using Epoche;
using System;
using System.Linq;
using System.Security.Cryptography;
using API_TRON.Exception;
using API_TRON.Model.Address;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using SimpleBase;

namespace API_TRON.Services
{
    public static class AddressService
    {
        public static AddressModel CreateAddress()
        {
            var keyPair = GenerateKeyPair();
            
            var privateKey = (keyPair.Private as ECPrivateKeyParameters)?.D.ToByteArrayUnsigned();
            var publicKey = (keyPair.Public as ECPublicKeyParameters)?.Q.GetEncoded();
            
            if (publicKey == null || privateKey == null)
            {
                throw new KeyPairException("Ключи равны null");
            }
            
            var hashKeccak = Keccak256.ComputeHash(publicKey.Skip(1).ToArray());
            var address = AddByteToArray(hashKeccak.Skip(12).ToArray(), Convert.ToByte(65));
            var hashFirst = GetHashSha256(address);
            var hashSecond = GetHashSha256(hashFirst);
            var addressWithCheckSum = AddLastBytesToArray(address, hashSecond.Take(4).ToArray());
            var addressBase58 = Base58.Bitcoin.Encode(addressWithCheckSum);
            
            return new AddressModel() {
                publicKey = ToHex(publicKey),
                privateKey = ToHex(privateKey),
                addressBase58 = addressBase58,
                address = ToHex(address)
            };
        }
        
        private static string ToHex(byte[] data) => String.Concat(data.Select(x => x.ToString("x2")));
        
        private static AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var curve = ECNamedCurveTable.GetByName("secp256k1");
            var domainParams = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            var secureRandom = new SecureRandom();
            var keyParams = new ECKeyGenerationParameters(domainParams, secureRandom);

            var generator = new ECKeyPairGenerator("ECDSA");
            generator.Init(keyParams);
            var keyPair = generator.GenerateKeyPair();
            return keyPair;
        }
        
        private static byte[] GetHashSha256(byte[] bytes)
        {
            var hashString = new SHA256Managed();
            byte[] hash = hashString.ComputeHash(bytes);
            return hash;
        }
        
        private static byte[] AddByteToArray(byte[] byteArray, byte newByte)
        {
            byte[] newArray = new byte[byteArray.Length + 1];
            byteArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
        
        private static byte[] AddLastBytesToArray(byte[] byteArray, byte[] newByteArray)
        {
            byte[] newArray = new byte[byteArray.Length + newByteArray.Length];
            byteArray.CopyTo(newArray, 0);
            for (int i = 0; i < newByteArray.Length; i++)
            {
                newArray[newArray.Length - newByteArray.Length + i] = newByteArray[i];
            }
            return newArray;
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