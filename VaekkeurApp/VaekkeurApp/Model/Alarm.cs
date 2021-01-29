using System;
using System.Collections.Generic;
using System.Text;

namespace VaekkeurApp.Model
{
    public class Alarm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeOffset {get; set; }

        public string Time { get; set; }

        public bool isActive { get; set; }
    }
}
