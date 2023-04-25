using Etherscan.DAL.Services.DataServices;
using Etherscan.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace Etherscan.Services
{
    public class TokenPriceService : BackgroundService
    {
        private readonly ILogger<TokenPriceService> _logger;
        private readonly BackgroundSettings _settings;
        private readonly TokenService _tokenService;

        public TokenPriceService(IOptions<BackgroundSettings> settings, ILogger<TokenPriceService> logger, TokenService tokenService)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenService = tokenService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("TokenPriceService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("TokenPriceService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("TokenPriceService background task is doing background work.");
                await PerformTask();
                try
                {
                    await Task.Delay(_settings.GracePeriodTime, stoppingToken);
                }
                catch (TaskCanceledException exception)
                {
                    _logger.LogCritical(exception, "TaskCanceledException Error", exception.Message);
                }
            }

            _logger.LogDebug("TokenPriceService background task is stopping.");
        }

        public async Task PerformTask()
        {
            var tokenList = await _tokenService.GetAllList(_settings.ConnectionString);

            _ = Parallel.ForEach(tokenList,new ParallelOptions { MaxDegreeOfParallelism = 50 }, async token =>
            {
                var price = await GetTokenPrice(token.Symbol);
                await _tokenService.UpdatePrice(token.Id, price, _settings.ConnectionString);
                _logger.LogDebug($"Token {token.Id} price update success");
            });


        }

        private async Task<decimal> GetTokenPrice(string symbol)
        {
            try
            {
                var client = new HttpClient();
                var path = $"https://min-api.cryptocompare.com/data/price?fsym={symbol}&tsyms=USD";
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var resp = await response.Content.ReadAsAsync<PriceResp>();


                    return resp.USD;
                }
            }
            catch
            {
                //Do nothing
            }

            return 0;
        }

    }
}
