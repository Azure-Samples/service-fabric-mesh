using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly string alphaVantageUriFormat = @"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey=meshdemo";

        public StockTicker()
        {
        }

        public void Open()
        {
            this.httpClient = new HttpClient();
            this.refreshInterval = TimeSpan.FromSeconds(15); // The alphavantage api only allows 5 calls per minute
            this.stockData = new List<StockData>();
            this.dataLock = new ReaderWriterLockSlim();
            this.cts = new CancellationTokenSource();
            this.UpdateInterval = TimeSpan.FromSeconds(5);
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

                data.LastKnownValue = this.GetStockPrice(await response.Content.ReadAsStringAsync());
                Console.WriteLine("symbol : {0} price : {1}", data.Symbol, data.LastKnownValue);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception when getting data for stock {0} - {1}", symbol, e);
            }


            return data;
        }

        private Uri GetStockQuoteUri(string symbol)
        {
            return new Uri(string.Format(this.alphaVantageUriFormat, symbol));
        }

        //
        // Data is of the form
        //        {
        //    "Global Quote": {
        //        "01. symbol": "^DJI",
        //        "02. open": "24770.2500",
        //        "03. high": "24916.1600",
        //        "04. low": "24445.1900",
        //        "05. price": "24688.3100",
        //        "06. volume": "505313987",
        //        "07. latest trading day": "2018-10-26",
        //        "08. previous close": "24984.5500",
        //        "09. change": "-296.2402",
        //        "10. change percent": "-1.1857%"
        //         }
        //      }
        //
        private string GetStockPrice(string rawString)
        {
            var jObject = JObject.Parse(rawString);
            foreach (var token in jObject)
            {
                if (token.Key.Contains("Global Quote"))
                {
                    if (token.Value.Type == JTokenType.Object)
                    {
                        var innerObject = (JObject)token.Value;
                        foreach (var innerToken in innerObject)
                        {
                            if (innerToken.Key.Contains("05. price"))
                            {
                                if (innerToken.Value.Type == JTokenType.String)
                                {
                                    return innerToken.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private string[] GetSymbols()
        {
            return new string[] { "DJI", "MSFT" };
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
