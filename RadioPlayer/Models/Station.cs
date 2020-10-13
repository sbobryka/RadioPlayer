using System;
using System.Collections.Generic;
using System.Text;

namespace RadioPlayer.Models
{
    internal class Station
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public Station(string name, string address)
        {
            Name = name;
            Address = address;
        }
    }
}
