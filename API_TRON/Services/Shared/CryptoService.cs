using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace API_TRON.Services.Shared
{
    public static class CryptoService
    {
        public static string ToHex(byte[] data) => String.Concat(data.Select(x => x.ToString("x2")));
        
        public static AsymmetricCipherKeyPair GenerateKeyPair()
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
        
        public static byte[] GetHashSha256(byte[] bytes)
        {
            var hashString = new SHA256Managed();
            byte[] hash = hashString.ComputeHash(bytes);
            return hash;
        }
        
        public static byte[] AddByteToArray(byte[] byteArray, byte newByte)
        {
            byte[] newArray = new byte[byteArray.Length + 1];
            byteArray.CopyTo(newArray, 1);
            newArray[0] = newByte;
            return newArray;
        }
        
        public static byte[] AddLastBytesToArray(byte[] byteArray, byte[] newByteArray)
        {
            byte[] newArray = new byte[byteArray.Length + newByteArray.Length];
            byteArray.CopyTo(newArray, 0);
            for (int i = 0; i < newByteArray.Length; i++)
            {
                newArray[newArray.Length - newByteArray.Length + i] = newByteArray[i];
            }
            return newArray;
        }
        
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}