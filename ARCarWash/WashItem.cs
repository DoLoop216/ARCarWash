using System;
using System.Collections.Generic;
using System.Text;

namespace ARCarWash
{
    class WashItem
    {
        private DateTime _Time = DateTime.Now;
        private string _Staff = "Undefined";
        private double _Price = 850;

        public string Staff { get => _Staff; set => _Staff = value; }
        public DateTime Time { get => _Time; set => _Time = value; }
        public double Price { get => _Price; set => _Price = value; }
    }
}
