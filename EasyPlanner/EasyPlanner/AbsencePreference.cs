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
    
    public partial class AbsencePreference
    {
        public int idTimeSlot { get; set; }
        public int dayOfWeek { get; set; }
        public System.TimeSpan startHour { get; set; }
        public System.TimeSpan endHour { get; set; }
        public System.DateTime firstDay { get; set; }
        public System.DateTime lastDate { get; set; }
        public int idPerson { get; set; }
    
        public virtual Person Person { get; set; }
    }
}
