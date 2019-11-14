using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;

namespace Output
{
    class Program
    {
        static void Main(string[] args)
        {
            var length = 100000;
            var files = 10;
            Console.WriteLine($"Writing {length} records.");
            var players = new string[] { "abera101", "jstro101", "jstro102", "mpete101", "mjone101", "kline101", "pharv101", "pharv102", "mharr101", "lgime101" };
            var types = new string[] {"Temperature","Magnetic","Pressure","Strain","Oxygen"};
            
            var holders = new List<Sensor>();
            var r = new Random();

            for (int j = 0; j < files; j++)
            {

                for (int i = 0; i < length; i++)
                {
                    var holder = new Sensor();
                    holder.Id = Guid.NewGuid();
                    //holder.Player = players[r.Next(0, 10)];
                    holder.Player = players[j];
                    holder.Node = r.Next(1, 40);
                    holder.NodeType = types[r.Next(0,5)];
                    holder.NodeValue = decimal.Round((decimal)(r.Next(0, 512) + r.NextDouble()), 2, MidpointRounding.AwayFromZero);
                    holder.EventTime = DateTime.UtcNow;
                    holder.PartitionId = r.Next(0, 8);
                    holder.EventEnqueuedUtcTime = holder.EventTime.AddMilliseconds(r.Next(10, 5000));
                    holder.EventProcessedUtcTime = holder.EventEnqueuedUtcTime.AddMilliseconds(r.Next(10, 100));
                    holders.Add(holder);                    
                }
                using (var writer = new StreamWriter($"files/sensor_{j.ToString("D2")}.csv"))
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.AutoMap<Sensor>();
                    csv.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "yyyy-MM-dd'T'HH:mm:ss.fffffffK" };

                    csv.Configuration.HasHeaderRecord = true;
                    csv.WriteRecords(holders);
                }
                Console.WriteLine("Write completed.");
                Console.WriteLine($"Cycle {j}");
                holders.Clear();
            }
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }

    class Sensor
    {
        public Guid Id { get; set; }
        public string Player { get; set; }
        public int Node { get; set; }
        public string NodeType { get; set; }
        public decimal NodeValue { get; set; }
        public DateTime EventTime { get; set; }
        public int PartitionId { get; set; }
        public DateTime EventEnqueuedUtcTime { get; set; }
        public DateTime EventProcessedUtcTime { get; set; }        
    }

}
