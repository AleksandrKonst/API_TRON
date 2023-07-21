using System;
using API_TRON.Exception;

namespace API_TRON.Services.Shared
{
    public static class CheckService
    {
        private const int accountLength = 34;
        
        public static string CheckAddress(string address)
        {
            if (address.Length != accountLength)
            {
                throw new AddressBadFormatException("Неверный формат адреса");
            }

            return address;
        }
        
        public static DateTime  CheckDataTime(string dateString)
        {
            var date = DateTime.Parse(dateString);
            if (date > DateTime.Now)
            {
                throw new DataTimeException("Неверный формат даты");
            }
            return date;
        }
    }
}