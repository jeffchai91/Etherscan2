namespace Etherscan.DAL.Entities.Data
{
    public class Token
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public int TotalHolders { get; set; }
        public decimal Price { get; set; }
        
    }
}
