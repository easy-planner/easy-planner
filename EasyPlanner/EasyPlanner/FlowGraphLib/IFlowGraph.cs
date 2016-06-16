using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyPlanner
{
    public interface IFlowGraph
    {


        
        FlowNode Source { get; set; }
        FlowNode Target { get; set; }
        List<FlowNode> Nodes { get; set; }
        List<FlowArc> Arcs { get; set; }
        Dictionary<FlowArc, double> Flows { get; set; }
        DateTime WeekStart { get; set;}
    }
}
