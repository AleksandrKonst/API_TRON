namespace API_TRON.Exception
{
    using System;
    
    public class ContractException : Exception
    {
        public ContractException(string message) : base(message)
        {
        }
    }
}