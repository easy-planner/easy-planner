//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasyPlanner
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public Person()
        {
            this.AbsenceDemands = new HashSet<AbsenceDemand>();
            this.AbsencePreferences = new HashSet<AbsencePreference>();
            this.WorkingShifts = new HashSet<WorkingShift>();
        }
    
        public int idPerson { get; set; }
        public string firstName { get; set; }
        public string name { get; set; }
        public string numberAVS { get; set; }
        public float occupancyRate { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public int idRole { get; set; }
    
        public virtual ICollection<AbsenceDemand> AbsenceDemands { get; set; }
        public virtual ICollection<AbsencePreference> AbsencePreferences { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<WorkingShift> WorkingShifts { get; set; }
    }
}
