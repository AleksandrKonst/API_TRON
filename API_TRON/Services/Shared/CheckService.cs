using System;
using API_TRON.Exception;

namespace API_TRON.Services.Shared
{
    public static class CheckService
    {
        private const int accountLength = 34;
        private const int priKeyLength = 64;
        
        public static string CheckAddress(string address)
        {
            if (address.Length != accountLength)
            {
                throw new AddressBadFormatException("Неверный формат адреса");
            }

            return address;
        }
        
        public static DateTime CheckDataTime(string dateString)
        {
            var date = DateTime.Parse(dateString);
            if (date > DateTime.Now)
            {
                throw new DataTimeException("Неверный формат даты");
            }
            return date;
        }
        
        public static string CheckPriKey(string priKey)
        {
            if (priKey.Length != priKeyLength)
            {
                throw new PriKeyException("Неверный формат ключа");
            }

            return priKey;
        }
    }
}