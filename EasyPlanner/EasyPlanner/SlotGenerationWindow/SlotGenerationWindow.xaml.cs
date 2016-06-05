/////////////////////////////////////////////////////////////
// Class responsible : Plinio Sacchetti
// Last update : 03.06.2016
// Sprint 5
// Story(ies) : Settings slotSchedule
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
using WpfScheduler;

namespace EasyPlanner
{
    /// <summary>
    /// Interaction logic for WeekGenerationWindow.xaml
    /// </summary>
    public partial class SlotGenerationWindow : Window
    {
        private bd_easyplannerEntities bdModel;
        public Boolean NextPrevButtonVisibity { get; set; }

        /// <summary>
        /// Construtor for the week generation Windows
        /// </summary>
        /// <param name="bd">Entity framework datbase</param>
        public SlotGenerationWindow(bd_easyplannerEntities bd)
        {
            InitializeComponent();
            this.bdModel = bd;
            bdModel = new bd_easyplannerEntities();
            NextPrevButtonVisibity = false;
            this.btnNext.Visibility = Visibility.Hidden;
            this.btnPrev.Visibility = Visibility.Hidden;

            //Scheduler settings
            slotGenerationScheduler.SelectedDate = DateTime.Today;
            slotGenerationScheduler.StartJourney = new TimeSpan(7, 0, 0);
            slotGenerationScheduler.EndJourney = new TimeSpan(19, 0, 0);
            slotGenerationScheduler.Mode = Mode.Week;

            //Activate double-click on the scheduler
            slotGenerationScheduler.OnEventDoubleClick += slotGenerationScheduler_OnEventDoubleClick;
            slotGenerationScheduler.OnScheduleDoubleClick += slotGenerationScheduler_OnScheduleDoubleClick;
        }


        public void LoadScheduleSlotFromDatabase()
        {
            //Clear events in the current scheduler
            PlanningGeneratorTools.ClearWorkingShiftScheduler(slotGenerationScheduler);

            DateTime d = slotGenerationScheduler.SelectedDate;
            //DateTime lastSunday = PlanningGeneratorTools.GetMondayBefore(d).AddDays(-1);
            DateTime lastSunday;
            //d is sunday ?
            if (d.DayOfWeek == DayOfWeek.Sunday)
                lastSunday = d.AddDays(-7);
            else
                lastSunday = PlanningGeneratorTools.GetSundayAfter(d).AddDays(-7);

            List<ScheduleSlot> scheduleSlotsWeek;

            //Get all the current week's scheduleSlots
            scheduleSlotsWeek = PlanningGeneratorTools.GetWeekScheduleSlots(d, bdModel);


            scheduleSlotsWeek = PlanningGeneratorTools.GetWeekScheduleSlots(d, bdModel);

            //Add the week's slot in the scheduler
            foreach (ScheduleSlot ss in scheduleSlotsWeek)
            {
                slotGenerationScheduler.AddEvent(new Event()
                {
                    Subject = "Date départ : " + ss.firstDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Date de fin : " + ss.lastDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Mininum : " + ss.minAttendency.ToString(),
                    Start = lastSunday.AddDays(ss.dayOfWeek).Add(ss.startHour),
                    End = lastSunday.AddDays(ss.dayOfWeek).Add(ss.endHour),
                    DayOfWeek = ss.dayOfWeek,
                    Color = new SolidColorBrush(Colors.Aqua),
                    FirstDay = ss.firstDay,
                    LastDay = ss.lastDay,
                    IdShift = ss.idTimeSlot,
                    MinAttendency = ss.minAttendency
                });
                this.btnNext.Visibility = Visibility.Visible;
                this.btnPrev.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Event occurs when there'is a double-click on an event.
        /// </summary>
        private void slotGenerationScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
            slotScheduleWin.lblPerson.Content = "";
            slotScheduleWin.ShowDialog();
        }

        /// <summary>
        /// Event occurs when there'is a double-click on a free space scheduler (not on an event).
        /// </summary>
        private void slotGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        {
            AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
            slotScheduleWin.lblPerson.Content = "";
            slotScheduleWin.ShowDialog();
        }

        /// <summary>
        /// Save to the database the slots showed in the scheduler.
        /// </summary>
        private void btnSlotScheduleGeneration_Click(object sender, RoutedEventArgs e)
        {
            //Create a ScheduleSlot foreach event in the scheduler
            foreach (Event ev in slotGenerationScheduler.Events)
            {
                if(ev.IdShift == 0) //it's a new slot
                {
                    bdModel.ScheduleSlots.Add(new ScheduleSlot()
                    {
                        dayOfWeek = ev.DayOfWeek,
                        firstDay = ev.FirstDay,
                        lastDay = ev.LastDay,
                        minAttendency = ev.MinAttendency,
                        startHour = new TimeSpan(ev.Start.Hour, ev.Start.Minute, 0),
                        endHour = new TimeSpan(ev.End.Hour, ev.End.Minute, 0),
                    });
                }
                else //otherwise update it
                {
                    ScheduleSlot ss = bdModel.ScheduleSlots.Single(s => s.idTimeSlot == ev.IdShift);
                    ss.dayOfWeek = ev.DayOfWeek;
                    ss.lastDay = ev.LastDay;
                    ss.firstDay = ev.FirstDay;
                    ss.minAttendency = ev.MinAttendency;
                    ss.startHour = new TimeSpan(ev.Start.Hour, ev.Start.Minute, 0);
                    ss.endHour = new TimeSpan(ev.End.Hour, ev.End.Minute, 0);
                }

            }
            bdModel.SaveChanges();
            MessageBox.Show("Traitement terminé", "Plages horaires sauvegardées dans la BD");
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (NextPrevButtonVisibity)
            {
                this.btnNext.Visibility = Visibility.Visible;
                this.btnPrev.Visibility = Visibility.Visible;
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            this.slotGenerationScheduler.PrevPage();
            LoadScheduleSlotFromDatabase();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.slotGenerationScheduler.NextPage();
            LoadScheduleSlotFromDatabase();
        }
    }
}
