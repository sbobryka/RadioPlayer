﻿using RadioPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace RadioPlayer.Controllers
{
    public class StationsController
    {
        public string FileName { get; }

        public StationsController(string fileName = "stations.json")
        {
            FileName = fileName;
        }

        public bool SaveToFile(IEnumerable<Station> stations)
        {
            string json = JsonSerializer.Serialize(stations);
            File.WriteAllText(FileName, json, Encoding.Default);
            return true;
        }

        public List<Station> LoadFromFile()
        {
            string json = File.ReadAllText(FileName, Encoding.Default);
            List<Station> stations = JsonSerializer.Deserialize<List<Station>>(json);
            return stations;
        }
    }
}
