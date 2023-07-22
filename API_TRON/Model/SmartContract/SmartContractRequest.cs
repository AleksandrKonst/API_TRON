namespace API_TRON.Model.SmartContract
{
    public class SmartContractRequest
    {
        public string contract_address { get; set; }
        public string parameter { get; set; }
        public string function_selector { get; set; }
        public string owner_address { get; set; }
        public bool visible { get; set; }
    }
}