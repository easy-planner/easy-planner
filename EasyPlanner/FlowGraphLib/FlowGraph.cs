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
using EasyPlanner;

namespace FlowGraphLib
{
    public class FlowGraph
    {
        public FlowNode Source { get; set; }
        public FlowNode Target { get; set; }
        public List<FlowNode> Nodes { get; set; }
        public List<FlowArc> Arcs { get; set; }
        public Dictionary<FlowArc,double> Flows { get; set; }
        public DateTime WeekStart { get; set; }

        /// <summary>
        /// Constructor of a flow graph
        /// </summary>
        /// <param name="persons">List of persons</param>
        /// <param name="slots">List of scheduleslots</param>
        /// <param name="start">Date of the first day of the week to generate</param>
        public FlowGraph(List<Person> persons, List<ScheduleSlot> slots, DateTime start)
        {
            this.Source = new FlowNode("Source");
            this.Target = new FlowNode("Target");
            this.WeekStart = start;

            // creating person's nodes
            foreach(Person person in persons)
            {
                PersonFlowNode node = new PersonFlowNode(person);
                this.Nodes.Add(node);
                // link node to source
                this.Arcs.Add(new FlowArc(this.Source,node,person.occupancyRate * 40));
            }

            // creating slots' nodes
            foreach(ScheduleSlot slot in slots)
            {
                SlotFlowNode node = new SlotFlowNode(slot);
                double delta = slot.endHour.Subtract(slot.startHour).TotalHours;
                this.Nodes.Add(node);
                // linking node to person's nodes
                foreach(FlowNode nody in this.Nodes)
                {
                    if(nody.Type == FlowNodeType.Person)
                    {
                        this.Arcs.Add(new FlowArc(nody, node, delta));
                    }
                }
                // linking node to target
                this.Arcs.Add(new FlowArc(node, this.Target, delta * slot.minAttendency));
            }
        }

        /// <summary>
        /// Simple reseter of arc's capacities before calculating
        /// </summary>
        private void ResetCapacity()
        {
            this.Flows = new Dictionary<FlowArc, double>();
            foreach(FlowArc arc in this.Arcs)
            {
                this.Flows[arc] = 0;
            }
        }

        /// <summary>
        /// Implementation of the Ford-Fulkerson algorithm
        /// in order to calculate maximum flow
        /// </summary>
        private void CalculateMaximumFlow()
        {
            this.ResetCapacity();
            // search path
            FlowPath path = this.PathFinder(this.Source, this.Target, new List<FlowNode>());
            // if path exists then augmentation of flows
            while(null != path)
            {
                this.Augment(path);
                // search new path
                path = this.PathFinder(this.Source, this.Target, new List<FlowNode>());
            }
        }

        /// <summary>
        /// Search of augmentating path
        /// </summary>
        /// <param name="source">Begining of the path</param>
        /// <param name="target">End of the path</param>
        /// <param name="visited">List of already visited nodes</param>
        /// <returns>An augmentating path OR null if none is found</returns>
        private FlowPath PathFinder(FlowNode source, FlowNode target, List<FlowNode> visited)
        {
            // adding source to list of visited nodes
            visited.Add(source);

            // trivial ending of recursion
            if(source == target)
            {
                FlowPath path = new FlowPath();
                path.AdditionalCapacity = float.MaxValue;
                return path;
            }

            // searching for ongoing path
            foreach(FlowArc arc in this.Arcs)
            {
                double delta = 0;
                FlowPath path = null;

                // is it a positive path ?
                if(arc.Start == source && this.Flows[arc] < arc.Capacity && !visited.Contains(arc.End))
                {
                    // recursive call on pathfinder
                    path = this.PathFinder(arc.End, target, new List<FlowNode>(visited));
                    delta = arc.Capacity - this.Flows[arc];
                }
                // is it a negative path ?
                if(arc.End == source && this.Flows[arc] > 0 && !visited.Contains(arc.Start))
                {
                    // recursive call on pathfinder
                    path = this.PathFinder(arc.Start, target, new List<FlowNode>(visited));
                    delta = this.Flows[arc];
                }

                // if an ongoing path has been found then
                if (null != path)
                {
                    // updating arcs and capacitiy
                    path.Arcs.Add(arc);
                    path.AdditionalCapacity = Math.Min(delta, path.AdditionalCapacity);
                    return path;
                }
            }
            // if no ongoing path found then return null
            return null;
        }

        /// <summary>
        /// Augmentation of the flows
        /// </summary>
        /// <param name="path">The augmentation path found</param>
        private void Augment(FlowPath path)
        {
            foreach(FlowArc arc in path.Arcs)
            {
                this.Flows[arc] += path.AdditionalCapacity;
            }
        }

        /// <summary>
        /// Generating of the workingshifts from the calculated flows
        /// </summary>
        /// <returns>List of coherents shifts given the constructed graph</returns>
        public List<WorkingShift> GetShifts()
        {
            List<WorkingShift> shifts = new List<WorkingShift>();

            CalculateMaximumFlow();

            // init quotas dict
            Dictionary<ScheduleSlot, List<Tuple<Person, double>>> quotas = new Dictionary<ScheduleSlot, List<Tuple<Person, double>>>();
            foreach(FlowNode node in this.Nodes)
            {
                if (node.Type == FlowNodeType.Slot)
                {
                    quotas.Add(((SlotFlowNode)node).Slot, new List<Tuple<Person, double>>());
                }
            }

            // recup quotas
            foreach(KeyValuePair<FlowArc,double> flowDict in this.Flows)
            {
                FlowArc arc = flowDict.Key; double flow = flowDict.Value;
                switch (arc.Start.Type)
                {
                    case FlowNodeType.Virtual:
                        // si flow < capacité alors personne pas complètement occupée => trop de RH
                        break;
                    case FlowNodeType.Person:
                        if(flow > 0)
                        {
                            quotas[((SlotFlowNode)arc.End).Slot].Add(new Tuple<Person, double>(((PersonFlowNode)arc.Start).Person, flow));
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
            foreach(KeyValuePair<ScheduleSlot, List<Tuple<Person, double>>> quotaDict in quotas)
            {
                ScheduleSlot slot = quotaDict.Key; List<Tuple<Person, double>> liste = quotaDict.Value;
                TimeSpan start = slot.startHour;
                TimeSpan end = slot.endHour;
                TimeSpan fromThere = start;
                foreach(Tuple<Person, double> quotaTuple in liste)
                {
                    Person person = quotaTuple.Item1; ; double hours = quotaTuple.Item2;
                    while(hours > 0)
                    {
                        TimeSpan toThere = TimeSpan.FromTicks(Math.Min(end.Ticks, fromThere.Add(TimeSpan.FromHours(hours)).Ticks));
                        WorkingShift shift = new WorkingShift();
                        DateTime theDay = WeekStart.AddDays(((int)slot.dayOfWeek - (int)WeekStart.DayOfWeek) % 7);
                        shift.start = theDay.Add(fromThere);
                        shift.end = theDay.Add(toThere);
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

            // merging contiguous shifts
            int i = 0;
            while(i < shifts.Count)
            {
                WorkingShift shiftI = shifts[i];
                int j = i+1;
                while(j < shifts.Count)
                {
                    WorkingShift shiftJ = shifts[j];
                    if(shiftJ.Person == shiftI.Person &&
                        shiftJ.start == shiftI.end)
                    {
                        shiftI.end = shiftJ.end;
                        shifts.Remove(shiftJ);
                    }
                    else
                    {
                        j++;
                    }
                }
                i++;
            }

            return shifts;
        }
    }
}
