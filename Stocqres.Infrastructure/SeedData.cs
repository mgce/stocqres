using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Stocqres.Domain;
using Stocqres.Domain.Enums;
using Stocqres.Infrastructure.Repositories.Api;

namespace Stocqres.Infrastructure
{
    public static class SeedData
    {
        private static Random _random;

        public static void Initialize(IContainer container)
        {
            _random = new Random();
            SeedStockExchange(container);
            SeedStocks(container);
            SeedStockGroupExchange(container);
        }

        public static void SeedStockExchange(IContainer container)
        {
            var stockExchangeRepository = container.Resolve<IStockExchangeRepository>();
            if (stockExchangeRepository.GetAsync(x => true).Result == null)
            {
                stockExchangeRepository.CreateAsync(new StockExchange());
            }
        }

        public static void SeedStocks(IContainer container)
        {
            var stockExchangeRepository = container.Resolve<IStockExchangeRepository>();
            var stockExchange = stockExchangeRepository.FindAsync(x => true).Result.Single();
            var stockRepository = container.Resolve<IStockRepository>();
            if (!stockRepository.FindAsync(x => true).Result.Any())
            {
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id, 
                    "Future Proccessing",
                    "FP",
                    1
                ));
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id,
                    "FP Lab",
                    "FPL",
                    100
                ));
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id,
                    "Progress Bar",
                    "PGB",
                    1
                ));
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id,
                    "FP Coin",
                    "FPC",
                    50
                ));
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id,
                    "FP Advanture",
                    "FPA",
                    50
                ));
                stockRepository.CreateAsync(new Stock(
                    stockExchange.Id,
                    "Deadline 24",
                    "DL24",
                    100
                ));
            }
        }

        public static void SeedStockGroupExchange(IContainer container)
        {
            var stockRepository = container.Resolve<IStockRepository>();
            var stocks = stockRepository.FindAsync(x => true).Result;

            var stockExchangeRepository = container.Resolve<IStockExchangeRepository>();
            var stockExchange = stockExchangeRepository.FindAsync(x => true).Result.Single();

            var stockGroupRepository = container.Resolve<IStockGroupRepository>();
            if (!stockGroupRepository.FindAsync(x => true).Result.Any())
            {
                foreach (var stock in stocks)
                {
                    stockGroupRepository.CreateAsync(new StockGroup(stockExchange.Id, StockOwner.StockExchange,
                        _random.Next(100, 10000),1, stock.Id));
                }
            }
        }
    }
}
