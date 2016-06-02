/////////////////////////////////////////////////////////////
// Class responsible : Greg
// Last update : 30.05.2016
// Sprint 4 and 5
// Story(ies) : Create an view Schedule slots, Update and delete Scheduled slots
/////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
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

namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class AddPlotWindow : Window
    {
        bd_easyplannerEntities bd;
        private ScheduleSlot current;
        private Boolean modified;

        public AddPlotWindow()
        {
            InitializeComponent();
            current = new ScheduleSlot();
            bd = new bd_easyplannerEntities();
            modified = false;
        }

        //Constructor for the "Update Plot Window", setting the previous data in the different fields
        /*public AddPlotWindow(ScheduleSlot ss)
        {
            
        }*/

        public AddPlotWindow(ScheduleSlot ss, bd_easyplannerEntities bd)
        {

            InitializeComponent();

            cbDayOfWeek.Text = ss.dayOfWeek;
            cbStartHourHour.Text = formatHoursMinutes(ss.startHour.Hours);
            cbStartHourMinute.Text = formatHoursMinutes(ss.startHour.Minutes);
            TimeSpan ts = (TimeSpan)ss.endHour;
            cbEndHourHour.Text = formatHoursMinutes(ts.Hours);
            cbEndHourMinute.Text = formatHoursMinutes(ts.Minutes);
            cbAttendency.Text = ss.minAttendency.ToString();

            if (ss.firstDay.ToString() != "")
            {
                DateTime dt = (DateTime)ss.firstDay;
                dpFirstDay.SelectedDate = dt;
            }
            if (ss.lastDay.ToString() != "")
            {
                DateTime dt = (DateTime)ss.lastDay;
                dpLastDay.SelectedDate = dt;
            }

            this.bd = bd;
            this.current = ss;
            current.idTimeSlot = ss.idTimeSlot;
            modified = true;

        }

        //Insert data in the database, not necessary to generate an ID
        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            current.dayOfWeek = cbDayOfWeek.Text;
            current.startHour = new TimeSpan(Int32.Parse(cbStartHourHour.Text), Int32.Parse(cbStartHourMinute.Text), 0);
            current.endHour = new TimeSpan(Int32.Parse(cbEndHourHour.Text), Int32.Parse(cbEndHourMinute.Text), 0);
            current.minAttendency = Int32.Parse(cbAttendency.Text);
            current.firstDay = dpFirstDay.SelectedDate;
            current.lastDay = dpLastDay.SelectedDate;

            if (!modified)
            {

                bd.ScheduleSlots.Add(current);
            }

            bd.SaveChanges();
            this.Close();
         }

        //Modify format of date and minutes. Add a 0 if h<10 and add 00 if m<10.
        //Converts data into String format to be displayed
        private String formatHoursMinutes(int t)
        {
            String s = "";
            if (t == 0)
            {
                s = "00";
            } else if (t < 10)
            {
                s = "0" + t.ToString();
            } else
            {
                s = t.ToString();
            }
            return s;
        }
    }
}
