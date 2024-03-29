﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingBot.BinanceServices;
using TradingBot.GoogleServices;
using TradingBot.OrderBook;

namespace TradingBot.OrderBookHeatingMap
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => {
                    services.AddTransient<OrderBookHeatingMapGenerator>();
                    services.AddTransient<IBinanceOrderBookConnector, BinanceOrderBookConnector>();
                    services.AddTransient<FutureMarket>();
                    services.AddTransient<IOrderBookBuilder, OrderBookBuilder>();
                    services.AddTransient<IOrderBook, BinanceOrderBook>();
                    services.AddTransient<IPriceRangeQuantizer, PriceRangeQuantizer>();
                    services.AddTransient<IGoogleSheetWriter, GoogleSheetWriter>();
                    services.AddTransient<OrderBookGoogleSheetUploader>();
                })
                .Build();

           
            CancellationTokenSource cts = new CancellationTokenSource();
            OrderBookHeatingMapGenerator orderBookHeatingMapGenerator = host.Services.GetRequiredService<OrderBookHeatingMapGenerator>();

            Console.WriteLine("Starting generation of orderbook...");
            Task orderBookGeneration = orderBookHeatingMapGenerator.Run("1PMYJvYX8ryckzLiH8xkDdrrYDi7Q64GbPNsMnLDATyE",23300, 23800, 30,1, 60, "BTCUSDT", cts.Token);
            Task keyread = Task.Run(() =>
            {
                char readChar = ' ';
                Console.WriteLine("Press Q to end the generation");
                while (readChar != 'Q')
                {
                    readChar = Console.ReadKey().KeyChar;
                }
                cts.Cancel();
            });
           
            await Task.WhenAll(orderBookGeneration, keyread);
        }
    }
}