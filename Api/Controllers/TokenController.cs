using Etherscan.DAL.Entities;
using Etherscan.DAL.Entities.Data;
using Etherscan.DAL.Services.DataServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Api.Extensions;
using Microsoft.Net.Http.Headers;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<TokenController> _logger;
        private IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly string _connection;
        private static int _pageSize = 10;

        public TokenController(ILogger<TokenController> logger, IConfiguration configuration, TokenService tokenService)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _connection = _configuration.GetValue<string>("ConnectionString");
        }


        [HttpGet]
        [Route("GetPaginateList")]
        public async Task<PaginateModal<List<TokenModel>>> GetPaginateList(int pageNo)
        {
            return await _tokenService.GetPaginateList(pageNo, _pageSize, _connection);
        }

        [HttpGet]
        [Route("GetTokenById")]
        public async Task<TokenModel> GetTokenById(int id)
        {
            return await _tokenService.GetTokenById(id, _connection);
        }

        [HttpPost(Name = "InsertToken")]
        public async Task<bool> InsertToken(Token token)
        {
            try
            {
                await _tokenService.Insert(token, _connection);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Failed to insert token");
                return false;
            }

            return true;
        }

        [HttpPost]
        [Route("UpdateToken")]
        public async Task<bool> UpdateToken(Token token)
        {
            try
            {
                await _tokenService.Update(token, _connection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert token");
                return false;
            }

            return true;
        }


        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download()
        {
            var dt = await _tokenService.GetDataTableList(_connection);
            var tokenListByte = (await _tokenService.GetDataTableList(_connection)).ToCsvByteArray();
            return new FileContentResult(tokenListByte, "application/octet-stream")
            {
                FileDownloadName = "TokenList.csv"
            };

        }

    }
}