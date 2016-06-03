using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfScheduler
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start {get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }
        public Brush Color { get; set; }
        public long IdShift { get; set; }
        public int MinAttendency { get; set; }
        public int DayOfWeek { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }

        public Event()
        {
            Id = Guid.NewGuid();
            AllDay = false;
        }
    }
}
