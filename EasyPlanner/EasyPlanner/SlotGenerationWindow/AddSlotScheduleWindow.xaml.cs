/////////////////////////////////////////////////////////////
// Class responsible : Greg
// Last update : 4.06.2016 by Plinio Sacchetti
//                  Implement-it in SlotGeneration Windows
// Sprint 4 and 5
// Story(ies) : Create an view Schedule slots, Update and delete Scheduled slots
/////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfScheduler;

namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class AddSlotScheduleWindow : Window
    {
        //bd_easyplannerEntities bd;
        private Scheduler scheduler;
        private Event current;
        private Boolean modified;
        private Guid idBckCurrent;

        /// create a ScheduleSlot
        /// Populate an enum with days of week
        /// </summary>
        /// <param name="scheduler">Scheduler to add schedule slot event</param>
        public AddSlotScheduleWindow(DateTime d, Scheduler scheduler)
        {
            InitializeComponent();
            this.scheduler = scheduler;
            current = new Event();
            
            modified = false;
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                cbDayOfWeekSlot.Items.Add(DateTimeFormatInfo.CurrentInfo.GetDayName(day));
            }
            
            cbDayOfWeekSlot.SelectedIndex = (int)d.DayOfWeek;
            current.Start = d;
            current.End = d;
            //dpFirstDaySlot.SelectedDate = DateTime.Today;
            //dpLastDaySlot.SelectedDate = DateTime.Today;
        }

        // <summary>
        /// Populate data in the fields with the existing ScheduleSlot
        /// set the (ScheduleSlot) current as the current scheduleslot
        /// </summary>
        /// <param name = "ss" >A Schedule slot already existing to get the data</param>
        /// <param name="scheduler">Scheduler to add schedule slot event</param>
        public AddSlotScheduleWindow(Event ss, WpfScheduler.Scheduler scheduler)
        {

            InitializeComponent();
            this.scheduler = scheduler;

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                cbDayOfWeekSlot.Items.Add(DateTimeFormatInfo.CurrentInfo.GetDayName(day));
            }
            cbDayOfWeekSlot.SelectedIndex = ss.DayOfWeek;
            cbStartHourHourSlot.Text = ss.Start.ToString("HH");
            cbStartHourMinuteSlot.Text = ss.Start.ToString("mm");
            cbEndHourHourSlot.Text = ss.End.ToString("HH"); 
            cbEndHourMinuteSlot.Text = ss.End.ToString("mm");
            cbAttendencySlot.Text = ss.MinAttendency.ToString();
            

            //if (ss.firstDay.ToString() != "")
            //{
            //    DateTime dt = (DateTime)ss.firstDay;
            //    dpFirstDaySlot.SelectedDate = dt;
            //}
            //if (ss.lastDay.ToString() != "")
            //{
            //    DateTime dt = (DateTime)ss.lastDay;
            //    dpLastDaySlot.SelectedDate = dt;
            //}

            //this.bd = bd;
            this.current = ss;
            idBckCurrent = ss.Id;
            modified = true;

        }

        // <summary>
        /// Insert data in the database
        /// if the record is already existing, it will update it
        /// if the record isnt existing, it will create it and add it
        /// </summary>
        /// <param>None</param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //if (isDateChecked() && areDatesCorrect())
            //{
            if (modified)
            {
                scheduler.DeleteEvent(idBckCurrent);
                current.Start = new DateTime(current.Start.Year, current.Start.Month, current.Start.Day, Int32.Parse(cbStartHourHourSlot.Text), Int32.Parse(cbStartHourMinuteSlot.Text), 0);
                current.End = new DateTime(current.End.Year, current.End.Month, current.End.Day, Int32.Parse(cbEndHourHourSlot.Text), Int32.Parse(cbEndHourMinuteSlot.Text), 0);
            }
            else
            {
                current.Start = current.Start.AddHours(Int32.Parse(cbStartHourHourSlot.Text));
                current.Start = current.Start.AddMinutes(Int32.Parse(cbStartHourMinuteSlot.Text));
                current.End = current.End.AddHours(Int32.Parse(cbEndHourHourSlot.Text));
                current.End = current.End.AddMinutes(Int32.Parse(cbEndHourMinuteSlot.Text));
            }
            current.DayOfWeek = cbDayOfWeekSlot.SelectedIndex;

            current.MinAttendency = Int32.Parse(cbAttendencySlot.Text);

            current.Color = new SolidColorBrush(Colors.Azure);
            current.Subject = "Date départ : " + current.Start.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Date de fin : " + current.Start.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Mininum : " + current.MinAttendency.ToString();

            //if (modified)
            //{
            //    scheduler.DeleteEvent(idBckCurrent);
            //    scheduler.AddEvent(current);

            //}
            //else
            scheduler.AddEvent(current);
                this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
            //        "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            //}
         }

        ///// <summary>
        ///// Check if the first day and last day are set
        ///// (if its not, then it will return false)
        ///// </summary>
        ///// <param>None</param>
        //private Boolean isDateChecked()
        //{
        //    Boolean isChecked = false;
        //    if (dpFirstDaySlot.SelectedDate != null && dpLastDaySlot.SelectedDate != null)
        //    {
        //        isChecked = true;
        //    }
        //    return isChecked;
        //}

        ///// <summary>
        ///// Check if the first day is earlier or equals to the last day
        ///// (if its not, then it will return false)
        ///// </summary>
        ///// <param>None</param>
        //private Boolean areDatesCorrect()
        //{
        //    Boolean areCorrect = false;
            
        //    if (dpFirstDaySlot.SelectedDate <= dpLastDaySlot.SelectedDate)
        //    {
        //        areCorrect = true;
        //    }
        //    return areCorrect;
        //}
    }
}
