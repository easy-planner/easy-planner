using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowGraphLib
{
    public class FlowArc
    {
        public FlowNode Start { get; set; }
        public FlowNode End { get; set; }
        public float Capacity { get; set; }
        public FlowArc(FlowNode start, FlowNode end, float capacity)
        {
            this.Start = start;
            this.End = end;
            this.Capacity = capacity;
        }
    }
}
