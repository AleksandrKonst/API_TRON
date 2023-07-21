namespace API_TRON.Exception
{
    using System;
    
    public class TransactionException : Exception
    {
        public TransactionException(string message) : base(message)
        {
        }
    }
}