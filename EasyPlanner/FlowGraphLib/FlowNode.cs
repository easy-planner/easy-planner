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
    public enum FlowNodeType
    {
        Virtual,Person,Slot
    }

    /// <summary>
    /// Basic node for Source and Target
    /// </summary>
    public class FlowNode
    {
        public string Name { get; set; }
        public FlowNodeType Type { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">an identificator (more for debugging than anything real)</param>
        public FlowNode(string name)
        {
            this.Name = name;
            this.Type = FlowNodeType.Virtual;
        }
    }

    /// <summary>
    /// Inheritated node for person's node
    /// </summary>
    public class PersonFlowNode : FlowNode
    {

        public Person Person { get; set; }
        public PersonFlowNode(string name) : base(name) { }

        /// <summary>
        /// Constructor
        /// adds a person's reference in addition of base constructor
        /// </summary>
        /// <param name="person">The Person</param>
        public PersonFlowNode(Person person) : base(person.ToString())
        {
            this.Person = person;
            this.Type = FlowNodeType.Person;
        }
    }

    /// <summary>
    /// Inheriated node for slot's node
    /// </summary>
    public class SlotFlowNode : FlowNode
    {
        public ScheduleSlot Slot { get; set; }

        /// <summary>
        /// Constructor
        /// adds a slot's reference in addition of base constuctor
        /// </summary>
        /// <param name="slot">The Slot</param>
        public SlotFlowNode(ScheduleSlot slot) : base(slot.ToString())
        {
            this.Slot= slot;
            this.Type = FlowNodeType.Slot;
        }
    }
}
