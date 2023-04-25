namespace Etherscan.DAL.Entities
{
    public class PaginateModal<T>
    {
        public int PageNo { get; set; }
        public long PageSize { get; set; }
        public long TotalItem { get; set; }
        public long TotalPage { get; set; }
        public T Data { get; set; }
    }

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
