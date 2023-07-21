namespace API_TRON.Exception
{
    using System;
    
    public class AddressBadFormatException : Exception
    {
        public AddressBadFormatException(string message) : base(message)
        {
        }
    }
}