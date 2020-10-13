using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace TestConsole
{
    class Program
    {
        static ICollection<Station> stations = new List<Station>();

        static void Main(string[] args)
        {
            Station station = new Station("station0", "address0");

            stations.Add(new Station("station1", "address1"));
            stations.Add(new Station("station2", "address2"));
            stations.Add(new Station("station3", "address3"));

            //var json = JsonSerializer.Serialize(station);
            //var test = JsonSerializer.Deserialize<Station>(json);

            var json = JsonSerializer.Serialize(stations);
            var list = JsonSerializer.Deserialize<List<Station>>(json);
        }
    }
}
