using Quartz;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyService
{
    public class CurrencySaver : IJob
    {
        private const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        private readonly object _lock = new object();
        public void Execute(IJobExecutionContext context)
        {
            string path = @"C:\Users\user\Desktop\MyTest.csv";
            string delimiter = ", ";
            Task consumer = Task.Factory.StartNew(() =>
            {
                lock (_lock)
                {
                    if (File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    string createText = "Name" + delimiter + "Value" + delimiter + "Date" + delimiter + Environment.NewLine;
                    File.WriteAllText(path, createText);
                    CreateCsv(path, delimiter);
                    Monitor.Pulse(_lock);
                }
            });
        }
        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;

        }

        public decimal CurrencyConversion(decimal amount, string fromCurrency, string toCurrency)
        {
            string url = string.Format(urlPattern, fromCurrency, toCurrency);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return (amount * exchangeRate);
            }
        }
        public void CreateCsv(string path, string delimiter)
        {
            var data = new[]
         {
                new Currency { DuoName = "USD/ILS", Value = CurrencyConversion(1,"USD","ILS"), UpdateDate = DateTime.Now},
                new Currency { DuoName = "GBP/EUR", Value = CurrencyConversion(1,"GBP","EUR"), UpdateDate = DateTime.Now},
                new Currency { DuoName = "EUR/JPY", Value = CurrencyConversion(1,"EUR","JPY"), UpdateDate = DateTime.Now},
                new Currency { DuoName = "EUR/USD", Value = CurrencyConversion(1,"EUR","USD"), UpdateDate = DateTime.Now}
            };
            foreach (var row in data)
            {
                string createText = row.DuoName + delimiter + row.Value + delimiter + row.UpdateDate + delimiter + Environment.NewLine;
                File.AppendAllText(path, createText.ToString());
            }
        }
    }
}

