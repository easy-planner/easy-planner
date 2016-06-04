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
            DateTime lastSunday = PlanningGeneratorTools.GetMondayBefore(d).AddDays(-1);

            //Get all the current week's scheduleSlots
            List<ScheduleSlot> scheduleSlotsWeek;
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
            slotScheduleWin.ShowDialog();
        }

        /// <summary>
        /// Event occurs when there'is a double-click on a free space scheduler (not on an event).
        /// </summary>
        private void slotGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        {
            AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
            slotScheduleWin.ShowDialog();
        }

        /// <summary>
        /// Save to the database the slots showed in the scheduler.
        /// </summary>
        private void btnSlotScheduleGeneration_Click(object sender, RoutedEventArgs e)
        {
            if (isDateChecked() && areDatesCorrect())
            {
                //gets first and last day
                DateTime firstDaySelection = (DateTime)dpFirstDay.SelectedDate;
                DateTime lastDaySelection = (DateTime)dpLastDay.SelectedDate;

                //Create a ScheduleSlot foreach event in the scheduler
                foreach (Event ev in slotGenerationScheduler.Events)
                {
                    if(ev.IdShift == 0) //it's a new slot
                    {
                        bdModel.ScheduleSlots.Add(new ScheduleSlot()
                        {
                            dayOfWeek = ev.DayOfWeek,
                            firstDay = firstDaySelection,
                            lastDay = lastDaySelection,
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
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpFirstDay.SelectedDate = PlanningGeneratorTools.GetMondayBefore(DateTime.Today);
            dpLastDay.SelectedDate = PlanningGeneratorTools.GetSundayAfter(DateTime.Today);

            if (NextPrevButtonVisibity)
            {
                this.btnNext.Visibility = Visibility.Visible;
                this.btnPrev.Visibility = Visibility.Visible;
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            this.slotGenerationScheduler.PrevPage();
            dpFirstDay.SelectedDate = ((DateTime)dpFirstDay.SelectedDate).AddDays(-7);
            dpLastDay.SelectedDate = ((DateTime)dpLastDay.SelectedDate).AddDays(-7);
            LoadScheduleSlotFromDatabase();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.slotGenerationScheduler.NextPage();
            dpFirstDay.SelectedDate = ((DateTime)dpFirstDay.SelectedDate).AddDays(7);
            dpLastDay.SelectedDate = ((DateTime)dpLastDay.SelectedDate).AddDays(7);
            LoadScheduleSlotFromDatabase();
        }
    }
}
