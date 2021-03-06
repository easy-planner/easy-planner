﻿/////////////////////////////////////////////////////////////
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
            dpFirstDay.SelectedDate = ss.FirstDay;
            dpLastDay.SelectedDate = ss.LastDay;

            this.current = ss;
            //Mise à 0 des heures
            current.Start=current.Start.AddHours(-current.Start.Hour);
            current.Start = current.Start.AddMinutes(-current.Start.Minute);
            current.End = current.End.AddHours(-current.End.Hour);
            current.End = current.End.AddMinutes(-current.End.Minute);

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
            if (isDateChecked() && areDatesCorrect())
            {
                current.DayOfWeek = cbDayOfWeekSlot.SelectedIndex;
                current.MinAttendency = Int32.Parse(cbAttendencySlot.Text);
                current.FirstDay = (DateTime)dpFirstDay.SelectedDate;
                current.LastDay = (DateTime)dpLastDay.SelectedDate;

                current.Start = current.Start.AddHours(Int32.Parse(cbStartHourHourSlot.Text));
                current.Start = current.Start.AddMinutes(Int32.Parse(cbStartHourMinuteSlot.Text));
                current.End = current.End.AddHours(Int32.Parse(cbEndHourHourSlot.Text));
                current.End = current.End.AddMinutes(Int32.Parse(cbEndHourMinuteSlot.Text));

                if (modified)
                {
                    scheduler.DeleteEvent(idBckCurrent);
                    current.Color = new SolidColorBrush(Colors.Coral);
                }
                else
                {
                    current.Color = new SolidColorBrush(Colors.OrangeRed);
                }


                current.Subject = "Date départ : " + current.FirstDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                    "Date de fin : " + current.LastDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                    "Mininum : " + current.MinAttendency.ToString();
                scheduler.AddEvent(current);
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                       "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
        }


        private void dpFirstDay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //dpLastDay.SelectedDate = dpFirstDay.SelectedDate;
        }

        //Faire une classe statique avec les méthodes ci-dessous car 
        // elles sont utilisées à plusieurs endroits du projet
        private Boolean isDateChecked()
        {
            Boolean isChecked = false;

            if (dpFirstDay.SelectedDate != null && dpLastDay.SelectedDate != null)
            {
                isChecked = true;
            }
            return isChecked;
        }

        private Boolean areDatesCorrect()
        {
            Boolean areCorrect = false;
            if (dpFirstDay.SelectedDate <= dpLastDay.SelectedDate)
            {
                areCorrect = true;
            }
            return areCorrect;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            current.DayOfWeek = cbDayOfWeekSlot.SelectedIndex;
            current.MinAttendency = Int32.Parse(cbAttendencySlot.Text);
            current.FirstDay = (DateTime)dpFirstDay.SelectedDate;
            current.LastDay = (DateTime)dpLastDay.SelectedDate;

            current.Start = current.Start.AddHours(Int32.Parse(cbStartHourHourSlot.Text));
            current.Start = current.Start.AddMinutes(Int32.Parse(cbStartHourMinuteSlot.Text));
            current.End = current.End.AddHours(Int32.Parse(cbEndHourHourSlot.Text));
            current.End = current.End.AddMinutes(Int32.Parse(cbEndHourMinuteSlot.Text));

            scheduler.DeleteEvent(idBckCurrent);
            current.Color = new SolidColorBrush(Colors.Red);
            //MessageBox.Show("La plage horaire selectionnée à bien été supprimée.");
            current.Subject = "Date départ : " + current.FirstDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                    "Date de fin : " + current.LastDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                    "Mininum : " + current.MinAttendency.ToString();
            scheduler.AddEvent(current);
            this.Close();
        }
    }
}
