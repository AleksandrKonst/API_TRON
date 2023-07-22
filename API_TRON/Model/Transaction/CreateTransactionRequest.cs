namespace API_TRON.Model.Transaction
{
    public class CreateTransactionRequest
    {
        public string owner_address { get; set; }
        public string to_address { get; set; }
        public int amount { get; set; }
    }
}