namespace API_TRON.Model.Transaction
{
    public class ErrorTransactionModel
    {
        public bool success { get; set; }
        public string error { get; set; }
        public int statusCode { get; set; }
    }
}