using Etherscan.DAL.Entities;
using Etherscan.DAL.Entities.Data;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace Etherscan.DAL.Services.DataServices
{
    public class TokenService
    {
        private readonly ILogger<TokenService> _logger;
        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }

        public async Task<int> Insert(Token entity, string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = @"
                    INSERT INTO etherscan.token
                    (symbol, name, total_supply, contract_address, total_holders, price)
                    VALUES(@symbol , @name, @totalSupply, @contractAddress, @totalHolders, @price);
                            ";
                MySqlCommand m = new MySqlCommand(query);
                m.Connection = conn;
                m.Parameters.AddWithValue("@symbol", entity.Symbol);
                m.Parameters.AddWithValue("@name", entity.Name);
                m.Parameters.AddWithValue("@totalSupply", entity.TotalSupply);
                m.Parameters.AddWithValue("@contractAddress", entity.ContractAddress);
                m.Parameters.AddWithValue("@totalHolders", entity.TotalHolders);
                m.Parameters.AddWithValue("@price", entity.Price);

                return await m.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<List<TokenModel>> GetList(int pageNo, int pageSize, string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = $@"
                    SELECT 
                    id, 
                    symbol, 
                    name, 
                    total_supply as totalSupply, 
                    contract_address as contractAddress, 
                    total_holders  as totalHolders, 
                    price,
                    total_supply / (select SUM(total_supply) from token) * 100 as totalSupplyPercentage
                    FROM token 

                    LIMIT {pageSize * (pageNo - 1)}, {pageSize}
                            ";
                return  conn.Query<TokenModel>(query).ToList();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<List<TokenModel>> GetAllList(string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = $@"
                    SELECT 
                    id, 
                    symbol, 
                    name, 
                    total_supply as totalSupply, 
                    contract_address as contractAddress, 
                    total_holders  as totalHolders, 
                    price,
                    total_supply / (select SUM(total_supply) from token) * 100 as totalSupplyPercentage
                    FROM token 
                            ";
                return conn.Query<TokenModel>(query).ToList();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<TokenModel> GetTokenById(int id, string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = $@"
                    SELECT 
                    id, 
                    symbol, 
                    name, 
                    total_supply as totalSupply, 
                    contract_address as contractAddress, 
                    total_holders  as totalHolders, 
                    price
                    FROM token
                    WHERE id = {id}";
                return conn.QuerySingle<TokenModel>(query);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<int> Update(Token entity, string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = $@"
                    UPDATE etherscan.token
                    SET symbol=@symbol, name=@name, total_supply=@totalSupply, contract_address=@contractAddress, total_holders=@totalHolders
                    WHERE id={entity.Id};";
                MySqlCommand m = new MySqlCommand(query);
                m.Connection = conn;
                m.Parameters.AddWithValue("@symbol", entity.Symbol);
                m.Parameters.AddWithValue("@name", entity.Name);
                m.Parameters.AddWithValue("@totalSupply", entity.TotalSupply);
                m.Parameters.AddWithValue("@contractAddress", entity.ContractAddress);
                m.Parameters.AddWithValue("@totalHolders", entity.TotalHolders);
                m.Parameters.AddWithValue("@price", entity.Price);

                return await m.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }

        public async Task<int> UpdatePrice(int id, decimal price, string connString)
        {
            using var conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                var query = $@"
                    UPDATE etherscan.token
                    SET price=@price
                    WHERE id={id};";
                MySqlCommand m = new MySqlCommand(query);
                m.Connection = conn;
                m.Parameters.AddWithValue("@price", price);

                return await m.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                throw;
            }
            finally
            {
                await conn.CloseAsync();
            }
        }


    }
}
