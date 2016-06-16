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
    class FlowPath
    {
        public double AdditionalCapacity { get; set; }
        public Dictionary<FlowArc,int> Arcs { get; set; }

        public FlowPath()
        {
            this.AdditionalCapacity = 0.0;
            this.Arcs = new Dictionary<FlowArc, int>();
        }

        public override string ToString()
        {
            string message = "";
            foreach(KeyValuePair<FlowArc,int> arcEntry in this.Arcs.Reverse())
            {
                message += ((arcEntry.Value == -1 ? "<neg>" : "") + arcEntry.Key.ToString() + "\n");
            }
            message += ("Add : " + this.AdditionalCapacity);
            return message;
        }
    }
}
