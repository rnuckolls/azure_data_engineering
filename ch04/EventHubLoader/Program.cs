using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace EventHubLoader
{
    class Program
    {
        static List<TimeSpan> times;
        private const string ConnectionString = "Endpoint=sb://de-dev-eastus-hubs.servicebus.windows.net/;SharedAccessKeyName=writer;SharedAccessKey=vaNNevw1Mff2m7/VGzSY16w8YNYNGJfQntp2gSAa154=;EntityPath=de-dev-eastus-hubs-loadtest2";

        static void Main(string[] args)
        {
            var totalTime = new TimeSpan();
            times = new List<TimeSpan>();
            var p = 8;
            Parallel.For(0, p, (i, state) =>
            {
                SendMessages(1000).GetAwaiter().GetResult();
            });

            Console.WriteLine("Done with Main");
            foreach (var ti in times)
            {
                totalTime = totalTime.Add(ti);
                Console.WriteLine(totalTime);
            }
            Console.WriteLine($"{p*1000} event in {totalTime.TotalSeconds}");
            Console.ReadLine();
        }

        private static async Task SendMessages(int count)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(ConnectionString);
            var Client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            var counter = 1;
            var fluctuation = 1;
            Console.WriteLine("Starting run ");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {                
                for (var i = 0; i < count; i++)
                {
                    try
                    {                        
                        Random rand = new Random();
                        if (counter > 50)
                        {
                            fluctuation = rand.Next(1, 20) <= 10 ? fluctuation = 1 : fluctuation = 20;
                            counter = 1;
                        }
                        var message = new MyObject() { ServiceId = rand.Next(1, 10), ServiceType = $"Type{i}", Duration = rand.Next(100, 1000) * fluctuation, PubDate = DateTime.UtcNow };
                        //Console.WriteLine($"Sending message: {i} fluctuation {fluctuation} duration {message.Duration}");
                        await Client.SendAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
                        counter += 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{DateTime.Now} > Exception: {ex.Message}");
                    }
                }
                times.Add(stopWatch.Elapsed);
                //Console.WriteLine($"End Run {stopWatch.Elapsed}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {ex.Message}");
            }
            finally
            {
                if (Client != null)
                {
                    await Client.CloseAsync();
                }
            }
            
        }
    }

    public class MyObject
    {
        public int ServiceId { get; set; }
        public string ServiceType { get; set; }
        public int Duration { get; set; }
        public DateTime PubDate { get; set; }
    }
}
