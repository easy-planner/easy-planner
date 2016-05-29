using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPlanner;

namespace FlowGraphLib
{
    public class FlowGraph
    {
        public FlowNode Source { get; set; }
        public FlowNode Target { get; set; }
        public List<FlowNode> Nodes { get; set; }
        public List<FlowArc> Arcs { get; set; }
        public Dictionary<FlowArc,float> Flows { get; set; }

        public FlowGraph(List<Person> persons, List<ScheduleSlot> slots)
        {
            this.Source = new FlowNode("Source");
            this.Target = new FlowNode("Target");

            foreach(Person person in persons)
            {
                PersonFlowNode node = new PersonFlowNode(person);
                this.Nodes.Add(node);
                this.Arcs.Add(new FlowArc(this.Source,node,person.occupancyRate * 40));
            }

            foreach(ScheduleSlot slot in slots)
            {
                SlotFlowNode node = new SlotFlowNode(slot);
                this.Nodes.Add(node);
                foreach(FlowNode nody in this.Nodes)
                {
                    if(nody.Type == FlowNodeType.Person)
                    {
                        this.Arcs.Add(new FlowArc(nody, node, slot.DeltaTime));
                    }
                }
                this.Arcs.Add(new FlowArc(node, this.Target, slot.DeltaTime * slot.minAttendency));
            }
        }

        private void CalculateMaximumFlow()
        {
            this.Flows = new Dictionary<FlowArc, float>();

        }

        public List<WorkingShift> GetShifts()
        {
            List<WorkingShift> shifts = new List<WorkingShift>();

            CalculateMaximumFlow();

            // init quotas dict
            Dictionary<ScheduleSlot, List<Tuple<Person, float>>> quotas = new Dictionary<ScheduleSlot, List<Tuple<Person, float>>>();
            foreach(FlowNode node in this.Nodes)
            {
                if (node.Type == FlowNodeType.Slot)
                {
                    quotas.Add(((SlotFlowNode)node).Slot, new List<Tuple<Person, float>>());
                }
            }

            // recup quotas
            foreach(KeyValuePair<FlowArc,float> flowDict in this.Flows)
            {
                FlowArc arc = flowDict.Key; float flow = flowDict.Value;
                switch (arc.Start.Type)
                {
                    case FlowNodeType.Virtual:
                        // si flow < capacité alors personne pas complètement occupée => trop de RH
                        break;
                    case FlowNodeType.Person:
                        if(flow > 0)
                        {
                            quotas[((SlotFlowNode)arc.End).Slot].Add(new Tuple<Person, float>(((PersonFlowNode)arc.Start).Person, flow));
                        }
                        break;
                    case FlowNodeType.Slot:
                        // si flow < capacité alors plage pas complètement couverte => pas assez de RH
                        break;
                    default:
                        break;
                }
            }

            // create shifts
            foreach(KeyValuePair<ScheduleSlot, List<Tuple<Person, float>>> quotaDict in quotas)
            {
                ScheduleSlot slot = quotaDict.Key; List<Tuple<Person, float>> liste = quotaDict.Value;
                TimeSpan start = slot.startHour;
                TimeSpan end = slot.endHour;
                TimeSpan fromThere = start;
                foreach(Tuple<Person, float> quotaTuple in liste)
                {
                    Person person = quotaTuple.Item1; ; double hours = quotaTuple.Item2;
                    while(hours > 0)
                    {
                        TimeSpan toThere = min(end, fromThere + hours);
                        WorkingShift shift = new WorkingShift();
                        shift.start = fromThere;
                        shift.end = toThere;
                        shift.Person = person;
                        shifts.Add(shift);
                        hours -= toThere.Subtract(fromThere).TotalHours;
                        if(toThere == end)
                        {
                            fromThere = start;
                        }else
                        {
                            fromThere = toThere;
                        }
                    }
                }
            }

            return shifts;
        }
    }

}
