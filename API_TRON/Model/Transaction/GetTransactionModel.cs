using System;

namespace API_TRON.Model.Transaction
{
    public class GetTransactionModel
    {
        public bool visible { get; set; }
        public string txID { get; set; }
        public RawData raw_data { get; set; }
        public string raw_data_hex { get; set; }
    }
    
    [Serializable]
    public class Contract
    {
        public Parameter parameter { get; set; }
        public string type { get; set; }
    }

    [Serializable]
    public class Parameter
    {
        public Value value { get; set; }
        public string type_url { get; set; }
    }

    public class RawData
    {
        public Contract[] contract { get; set; }
        public string ref_block_bytes { get; set; }
        public string ref_block_hash { get; set; }
        public long expiration { get; set; }
        public long timestamp { get; set; }
    }

    public class Value
    {
        public int amount { get; set; }
        public string owner_address { get; set; }
        public string to_address { get; set; }
    }
}