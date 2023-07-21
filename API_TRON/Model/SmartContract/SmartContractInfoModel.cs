namespace API_TRON.Model.SmartContract
{
    public class SmartContractInfoModel
    {
        public Result result { get; set; }
        public int energy_used { get; set; }
        public string[] constant_result { get; set; }
        public Transaction transaction { get; set; }
    }

    public class Result
    {
        public bool result { get; set; }
    }
    
    public class Ret
    {
    }
    
    public class Transaction
    {
        public Ret[] ret { get; set; }
        public bool visible { get; set; }
        public string txID { get; set; }
        public RawData raw_data { get; set; }
        public string raw_data_hex { get; set; }
    }
    
    public class RawData
    {
        public Contract[] contract { get; set; }
        public string ref_block_bytes { get; set; }
        public string ref_block_hash { get; set; }
        public long expiration { get; set; }
        public long timestamp { get; set; }
    }
    
    public class Contract
    {
        public Parameter parameter { get; set; }
        public string type { get; set; }
    }
    
    public class Value
    {
        public string data { get; set; }
        public string owner_address { get; set; }
        public string contract_address { get; set; }
    }
    
    public class Parameter
    {
        public Value value { get; set; }
        public string type_url { get; set; }
    }
}