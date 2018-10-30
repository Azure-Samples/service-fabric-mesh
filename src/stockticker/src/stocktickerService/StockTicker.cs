// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Microsoft.ServiceFabricMesh.Samples.Stockticker.Service
{
    public class StockTicker : IDisposable
    {
        private HttpClient httpClient;
        private TimeSpan refreshInterval;
        private List<StockData> stockData;
        private ReaderWriterLockSlim dataLock;
        private Task dataRefresher;
        private CancellationTokenSource cts;
        private readonly string iexTradingUriFormat = @"https://api.iextrading.com/1.0/stock/{0}/quote";
	private readonly string[] defaultTickers = { "MSFT", " AAPL ", "GOOG"};
	private const string tickerFileLinux = @"/var/settings/tickers";
	private const string tickerFileWindows = @"c:\settings\tickers";
        private const string tickerDataRefreshIntervalEnvVar = "DATA_REFRESH_INTERVAL_SECONDS";
        private const string tickerDisplayUpdateIntervalEnvVar = "DISPLAY_REFRESH_INTERVAL_SECONDS";

        public StockTicker()
        {
        }

        public void Open()
        {
            this.httpClient = new HttpClient();
            this.refreshInterval = this.GetIntervalFromEnvVar(tickerDataRefreshIntervalEnvVar, 10);
            this.stockData = new List<StockData>();
            this.dataLock = new ReaderWriterLockSlim();
            this.cts = new CancellationTokenSource();
            this.UpdateInterval = this.GetIntervalFromEnvVar(tickerDisplayUpdateIntervalEnvVar, 5);
            this.dataRefresher = Task.Run(() => this.RefreshStockData(this.cts.Token));
        }

        public TimeSpan UpdateInterval { get; private set; }

        public void Close()
        {
            this.cts.Cancel();
            try
            {
                this.dataRefresher.Wait();
            }
            catch(Exception e)
            {
                if (!(e is TaskCanceledException))
                {
                    Console.WriteLine("Got Exception during Close : {0}", e);
                }
                // swallow TaskCancelled
            }
        }

        public string GetStockData()
        {
            try
            {
                dataLock.EnterReadLock();
                return JsonConvert.SerializeObject(this.stockData);
            }
            finally
            { 
                dataLock.ExitReadLock();
            }
        }

        private async Task RefreshStockData(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                var symbols = GetSymbols();
                List<StockData> currentData = new List<StockData>();

                foreach (var symbol in symbols)
                {
                    currentData.Add(await GetStockData(symbol, token));
                }

                {
                    dataLock.EnterWriteLock();
                    this.stockData = currentData;
                    dataLock.ExitWriteLock();
                }

                await Task.Delay(this.refreshInterval, token);
            }
        }

        private async Task<StockData> GetStockData(string symbol, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            StockData data = new StockData() { Symbol = symbol };

            try
            {
                var response = await this.httpClient.GetAsync(GetStockQuoteUri(symbol));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got error when getting data for stock {0} - Status Code {1} : {2}", symbol, response.StatusCode, response.ReasonPhrase);
                    return data;
                }

                this.ParseStockData(await response.Content.ReadAsStringAsync(), ref data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception when getting data for stock {0} - {1}", symbol, e);
            }


            return data;
        }

        private Uri GetStockQuoteUri(string symbol)
        {
            return new Uri(string.Format(this.iexTradingUriFormat, symbol.Trim()));
        }

        //
        // Data is of the form
        //{"symbol":"MSFT","companyName":"Microsoft Corporation","primaryExchange":"Nasdaq Global Select","sector":"Technology","calculationPrice":"close","open":105.63,"openTime":1540560600514,"close":106.96,"closeTime":1540584000410,"high":108.75,"low":104.76,"latestPrice":106.96,"latestSource":"Close","latestTime":"October 26, 2018","latestUpdate":1540584000410,"latestVolume":55471497,"iexRealtimePrice":null,"iexRealtimeSize":null,"iexLastUpdated":null,"delayedPrice":106.96,"delayedPriceTime":1540584000410,"extendedPrice":107,"extendedChange":0.04,"extendedChangePercent":0.00037,"extendedPriceTime":1540587388338,"previousClose":108.3,"change":-1.34,"changePercent":-0.01237,"iexMarketPercent":null,"iexVolume":null,"avgTotalVolume":34188213,"iexBidPrice":null,"iexBidSize":null,"iexAskPrice":null,"iexAskSize":null,"marketCap":821048356003,"peRatio":28.68,"week52High":116.18,"week52Low":80.7,"ytdChange":0.24817183499818496}
        //
        private class Quote
        {
            public Quote()
            {
                this.companyName = "";
                this.latestPrice = "";
            }

            public string companyName;
            public string latestPrice;
        }

        private void ParseStockData(string rawString, ref StockData data)
        {
            var quote = JsonConvert.DeserializeObject<Quote>(rawString);
            data.CompanyName = quote.companyName;
            data.LastKnownValue = quote.latestPrice;
        }

        private string[] GetSymbols()
        {
            string tickerFile;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
	    {
                tickerFile = tickerFileLinux;
            }
            else
            {
                tickerFile = tickerFileWindows;
            }

            if (!File.Exists(tickerFile))
            {
                return defaultTickers;
            }

            return this.ReadTickerFile(tickerFile);
        }

        private string[] ReadTickerFile(string tickerFile)
        {
            try
            {
                // The contents of the file are comma separated list of symbols
                var tickerFileContents = File.ReadAllText(tickerFile);
                if (String.IsNullOrEmpty(tickerFileContents))
                {
                    return new string[]{};
                }

                return tickerFileContents.Split(',');
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught exception during ReadTickerFile {0} : exception {1}", tickerFile, e);
            }

            return new string[]{};
        }

        private TimeSpan GetIntervalFromEnvVar(string varName, int defaultValueInSeconds)
        {
            if (!int.TryParse(Environment.GetEnvironmentVariable(varName), out int interval))
            {
                interval = defaultValueInSeconds;
            }

            return TimeSpan.FromSeconds(interval);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StockTicker() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
