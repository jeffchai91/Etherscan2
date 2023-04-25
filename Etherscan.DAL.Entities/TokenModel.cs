namespace Etherscan.DAL.Entities
{
    public class TokenModel
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public long TotalSupply { get; set; }
        public string ContractAddress { get; set; }
        public int TotalHolders { get; set; }
        public decimal Price { get; set; }
        public decimal TotalSupplyPercentage { get; set; }
    }
}
