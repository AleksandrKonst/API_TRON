namespace API_TRON.Exception
{
    using System;
    
    public class KeyPairException : Exception
    {
        public KeyPairException(string message) : base(message)
        {
        }
    }
}