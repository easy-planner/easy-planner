/////////////////////////////////////////////////////////////
// Class responsible : Plinio Sacchetti
// Last update : 02.06.2016
// Sprint 5
// Story(ies) : Generate a scheduler
/////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfScheduler;

namespace EasyPlanner
{
    /// <summary>
    /// Tools usefull for Scheduler Genration.
    /// CRUD sheduler.
    /// 
    /// </summary>
    public static class PlanningGeneratorTools
    {
        /// <summary>
        /// Add WorkingShift (events) in the scheduler. WorkingShift is for an employed.
        /// </summary>
        /// <param name="wsList">List of WorkingShift</param>
        /// <param name="scheduler">Scheduler, see WpfScheduler for more...</param>
        public static void AddWorkingShiftScheduler(List<WorkingShift> wsList, Scheduler scheduler)
        {
            foreach (WorkingShift ws in wsList)
            {
                scheduler.AddEvent(new Event()
                {
                    Subject = ws.Person.firstName + " " + ws.Person.name,
                    Description = ws.description,
                    Color = colorPiker(ws.Person),
                    Start = (System.DateTime)ws.start,
                    End = (System.DateTime)ws.end,
                });
            }
        }
        /// <summary>
        /// Define a unique color denpending on a id
        /// </summary>
        /// <param name="p">Person, get the id from the person</param>
        /// <returns>Brush, painting color</returns>
        private static Brush colorPiker(Person p)
        {
            return (Brush)new BrushConverter().ConvertFromString(typeof(Brushes).GetProperties()[p.idPerson % (typeof(Brushes).GetProperties().Count())].Name);
        }

        /// <summary>
        /// Clear all events in the scheduler
        /// </summary>
        /// <param name="scheduler">Scheduler, see WpfScheduler for more...</param>
        public static void ClearWorkingShiftScheduler(Scheduler scheduler)
        {
            for (int i = scheduler.Events.Count - 1; i >= 0; i--)
                scheduler.DeleteEvent(scheduler.Events[i].Id);
        }
        /// <summary>
        /// Remove from schelduler the workingshifts listed in the paramater.
        /// (if the workingShif id is in the list then it is removed)
        /// </summary>
        /// <param name="wsList">List of WorkingShift to remove</param>
        /// <param name="bd">database</param>
        public static void RemoveWorkingShiftScheduler(List<WorkingShift> wsList, Scheduler scheduler)
        {
            foreach (WorkingShift ws in wsList)
            {
                Event eve = scheduler.Events.Single(ev => ev.IdShift == ws.idShift);
                scheduler.DeleteEvent(eve.Id);
            }
        }

        /// <summary>
        /// Remove from schelduler the workingshifts for a specific week in the paramater.
        /// (if the workingShif id is in the list then it is removed)
        /// </summary>
        /// <param name="d">date in the removal week</param>
        /// <param name="bd">database</param>
        public static void RemoveWeekWorkingShiftScheduler(DateTime d, Scheduler scheduler)
        {
            // lastMonday is always the Monday before nextSunday.
            // When today is a Sunday, lastMonday will be tomorrow.     
            int offset = d.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = d.AddDays(-offset);
            DateTime nextMonday = lastMonday.AddDays(7);

            List<Event> eventsToRemove = scheduler.Events.Where(ev => ev.Start >= lastMonday && ev.Start <= nextMonday).ToList();

            foreach (Event ev in eventsToRemove)
                scheduler.DeleteEvent(ev.Id);
        }


        /// <summary>
        /// Remove from database the workingshifts listed in the paramater.
        /// (if the workingShif id is in the list then it is removed)
        /// </summary>
        /// <param name="wsList">List of WorkingShift to remove</param>
        /// <param name="bd">database</param>
        public static void RemoveWorkingShiftDataBase(List<WorkingShift> wsList, bd_easyplannerEntities bd)
        {

            WorkingShift wsRemove=null;
            foreach (WorkingShift ws in wsList)
            {
                wsRemove = bd.WorkingShifts.Find(ws.idShift);
                if (wsRemove != null)
                    bd.WorkingShifts.Remove(wsRemove);
            }

            bd.SaveChanges();
        }

        /// <summary>
        /// Save in the database the workingshfits
        /// </summary>
        /// <param name="wsList">List of WorkingShift</param>
        /// <param name="bd">database</param>
        public static void PersistWorkingShiftDataBase(List<WorkingShift> wsList, bd_easyplannerEntities bd)
        {
            foreach (WorkingShift ws in wsList)
                bd.WorkingShifts.Add(ws);
            bd.SaveChanges();
        }

        /// <summary>
        /// Get the slot schedule (opening hours slot) for a specified week (monday to sunday)
        /// </summary>
        /// /// <param name="d">A date in the week</param>
        /// <param name="bd">Database</param>
        /// <returns>List of ScheduleSlot for a week</returns>
        public static List<ScheduleSlot> GetWeekScheduleSlots(DateTime d, bd_easyplannerEntities bd)
        {
            // lastMonday is always the Monday before nextSunday.
            // When today is a Sunday, lastMonday will be tomorrow.     
            int offset = d.DayOfWeek - DayOfWeek.Monday;
            DateTime lastMonday = d.AddDays(-offset);
            DateTime nextSunday = lastMonday.AddDays(6);

            // return all slot between monday and sunday
            return bd.ScheduleSlots.Where(s => s.lastDay >= lastMonday && s.firstDay <= nextSunday).ToList();
        }
    }
}
