using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyPlanner;

namespace FlowGraphLib
{
    public enum FlowNodeType
    {
        Virtual,Person,Slot
    }

    public class FlowNode
    {
        public string Name { get; set; }
        public FlowNodeType Type { get; set; }
        public FlowNode(string name)
        {
            this.Name = name;
            this.Type = FlowNodeType.Virtual;
        }
    }

    public class PersonFlowNode : FlowNode
    {

        public Person Person { get; set; }
        public PersonFlowNode(string name) : base(name) { }
        public PersonFlowNode(Person person) : base(person.ToString())
        {
            this.Person = person;
            this.Type = FlowNodeType.Person;
        }
    }

    public class SlotFlowNode : FlowNode
    {
        public ScheduleSlot Slot { get; set; }
        public SlotFlowNode(string name) : base(name) { }
        public SlotFlowNode(ScheduleSlot slot) : base(slot.ToString())
        {
            this.Slot= slot;
            this.Type = FlowNodeType.Slot;
        }
    }
}
