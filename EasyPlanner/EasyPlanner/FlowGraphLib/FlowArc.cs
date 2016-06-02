/////////////////////////////////////////////////////////////
// Class responsible : Alexandre
// Last update : 02.06.2016
// Sprint 5
// Story(ies) : Generate Scheduler
/////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlanner
{
    public class FlowArc
    {
        public FlowNode Start { get; set; }
        public FlowNode End { get; set; }
        public double Capacity { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Origine node</param>
        /// <param name="end">Destination node</param>
        /// <param name="capacity">Arc's capacity</param>
        public FlowArc(FlowNode start, FlowNode end, double capacity)
        {
            this.Start = start;
            this.End = end;
            this.Capacity = capacity;
        }
    }
}
