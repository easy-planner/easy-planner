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
        public List<FlowArc> Arcs { get; set; }

        public FlowPath()
        {
            this.AdditionalCapacity = 0.0;
            this.Arcs = new List<FlowArc>();
        }
    }
}
