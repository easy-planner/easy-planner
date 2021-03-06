﻿/////////////////////////////////////////////////////////////
// Class responsible : Greg
// Last update : 30.05.2016
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

namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class AddPlotWindow : Window
    {
        bd_easyplannerEntities bdModel;
        private ScheduleSlot current;
        private Boolean modified;


        // <summary>
        /// create a ScheduleSlot
        /// Populate an enum with days of week
        /// </summary>
        /// <param>None</param>
        public AddPlotWindow()
        {
            InitializeComponent();
            current = new ScheduleSlot();
            bdModel = bd_easyplannerEntities.OpenWithFallback();
            modified = false;
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                cbxDayOfWeek.Items.Add(DateTimeFormatInfo.CurrentInfo.GetDayName(day));
            }
            cbxDayOfWeek.SelectedIndex = 1;
            dpFirstDay.SelectedDate = DateTime.Today;
            dpLastDay.SelectedDate = DateTime.Today;
        }

        // <summary>
        /// Populate data in the fields with the existing ScheduleSlot
        /// set the (ScheduleSlot) current as the current scheduleslot
        /// </summary>
        /// <param name = "ss" >A Schedule slot already existing to get the data</param>
        /// /// <param name = "bd" >the current easyplanner entity</param>
        public AddPlotWindow(ScheduleSlot ss, bd_easyplannerEntities bd)
        {

            InitializeComponent();
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                cbxDayOfWeek.Items.Add(DateTimeFormatInfo.CurrentInfo.GetDayName(day));
            }
            cbxDayOfWeek.SelectedIndex = ss.dayOfWeek;
            cbxStartHourHour.Text = formatHoursMinutes(ss.startHour.Hours);
            cbxStartHourMinute.Text = formatHoursMinutes(ss.startHour.Minutes);
            cbxEndHourHour.Text = formatHoursMinutes(ss.endHour.Hours);
            cbxEndHourMinute.Text = formatHoursMinutes(ss.endHour.Minutes);
            cbxAttendency.Text = ss.minAttendency.ToString();

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

            this.bdModel = bd;
            this.current = ss;
            current.idTimeSlot = ss.idTimeSlot;
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
            if (isDateChecked() && areDatesCorrect())
            {
                current.dayOfWeek = cbxDayOfWeek.SelectedIndex;
                current.startHour = new TimeSpan(Int32.Parse(cbxStartHourHour.Text), Int32.Parse(cbxStartHourMinute.Text), 0);
                current.endHour = new TimeSpan(Int32.Parse(cbxEndHourHour.Text), Int32.Parse(cbxEndHourMinute.Text), 0);
                current.minAttendency = Int32.Parse(cbxAttendency.Text);
                current.firstDay = (DateTime)dpFirstDay.SelectedDate;
                current.lastDay = (DateTime)dpLastDay.SelectedDate;

                if (!modified)
                {
                    bdModel.ScheduleSlots.Add(current);
                }

                bdModel.SaveChanges();
                this.Close();
            } else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
         }

        /// <summary>
        /// Check if the first day and last day are set
        /// (if its not, then it will return false)
        /// </summary>
        /// <param>None</param>
        private Boolean isDateChecked()
        {
            Boolean isChecked = false;
            if (dpFirstDay.SelectedDate != null && dpLastDay.SelectedDate != null)
            {
                isChecked = true;
            }
            return isChecked;
        }

        /// <summary>
        /// Check if the first day is earlier or equals to the last day
        /// (if its not, then it will return false)
        /// </summary>
        /// <param>None</param>
        private Boolean areDatesCorrect()
        {
            Boolean areCorrect = false;
            if (dpFirstDay.SelectedDate <= dpLastDay.SelectedDate)
            {
                areCorrect = true;
            }
            return areCorrect;
        }


        /// <summary>
        /// Modify format of date and minutes. Add a 0 if h<10 and add 00 if m<10
        /// Converts data into String format to be displayed
        /// </summary>
        /// <param name = "t" >a time in INT to convert to string</param>
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

        private void dpFirstDay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpLastDay.SelectedDate = dpFirstDay.SelectedDate;
        }
    }
}
