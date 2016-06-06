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
    public partial class AbsencePreferenceGenerationWindow : Window
    {
        private bd_easyplannerEntities bdModel;
        public Boolean NextPrevButtonVisibity { get; set; }

        /// <summary>
        /// Construtor for the week generation Windows
        /// </summary>
        /// <param name="bd">Entity framework datbase</param>
        public AbsencePreferenceGenerationWindow(bd_easyplannerEntities bd)
        {
            InitializeComponent();
            this.bdModel = bd;

            bdModel = bd_easyplannerEntities.OpenWithFallback();
            
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

            List<AbsencePreference> scheduleAbsPreferencesSlotsWeek;

            //Get all the current week's scheduleSlots
            scheduleAbsPreferencesSlotsWeek = PlanningGeneratorTools.GetAbsPreferenceSlots(d, bdModel);

            //Add the week's slot in the scheduler
            foreach (AbsencePreference ss in scheduleAbsPreferencesSlotsWeek)
            {
                slotGenerationScheduler.AddEvent(new Event()
                {
                    Subject = ss.idPerson.ToString() + " " + ss.Person.firstName + " " + ss.Person.name + Environment.NewLine + Environment.NewLine +
                                "Date départ : " + ss.firstDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Date de fin : " + ss.lastDate.ToString("MM/dd/yyyy"),
                    Start = lastSunday.AddDays(ss.dayOfWeek).Add(ss.startHour),
                    End = lastSunday.AddDays(ss.dayOfWeek).Add(ss.endHour),
                    DayOfWeek = ss.dayOfWeek,
                    Color = new SolidColorBrush(Colors.Aqua),
                    FirstDay = ss.firstDay,
                    LastDay = ss.lastDate,
                    IdShift = ss.idTimeSlot,
                });
            }
            cbxPeople.SelectedIndex = -1;
            btnSlotScheduleGeneration.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Event occurs when there'is a double-click on an event.
        /// </summary>
        private void slotGenerationScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            if (cbxPeople.SelectedIndex != -1)
            {
                AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
                slotScheduleWin.lblAttendencySlot.Visibility = Visibility.Hidden;
                slotScheduleWin.cbAttendencySlot.Visibility = Visibility.Hidden;
                slotScheduleWin.ShowDialog();
                btnSlotScheduleGeneration.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("Veuillez choisir un employé dans la liste déroulante !");

        }

        /// <summary>
        /// Event occurs when there'is a double-click on a free space scheduler (not on an event).
        /// </summary>
        private void slotGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        {
            Person p = bdModel.AbsencePreferences.First(a => a.idTimeSlot == e.IdShift).Person;
            if (cbxPeople.SelectedIndex == -1)
            {
                AddAbsencePreferenceOnScheduler(p);
                cbxPeople.SelectedValue = p.idPerson;
            }
            else
            {
                AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
                slotScheduleWin.lblPerson.Content = "Employé : " + p.firstName + " " + p.name;
                slotScheduleWin.lblAttendencySlot.Visibility = Visibility.Hidden;
                slotScheduleWin.cbAttendencySlot.Visibility = Visibility.Hidden;
                slotScheduleWin.ShowDialog();
                btnSlotScheduleGeneration.Visibility = Visibility.Visible;
            }
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
                    bdModel.AbsencePreferences.Add(new AbsencePreference()
                    {
                        dayOfWeek = ev.DayOfWeek,
                        firstDay = ev.FirstDay,
                        lastDate = ev.LastDay,
                        startHour = new TimeSpan(ev.Start.Hour, ev.Start.Minute, 0),
                        endHour = new TimeSpan(ev.End.Hour, ev.End.Minute, 0),
                        idPerson = ((Person)(cbxPeople.SelectedItem)).idPerson
                });
                }
                else //otherwise update it
                {
                    AbsencePreference ss = bdModel.AbsencePreferences.Single(s => s.idTimeSlot == ev.IdShift);
                    ss.dayOfWeek = ev.DayOfWeek;
                    ss.lastDate = ev.LastDay;
                    ss.firstDay = ev.FirstDay;
                    ss.startHour = new TimeSpan(ev.Start.Hour, ev.Start.Minute, 0);
                    ss.endHour = new TimeSpan(ev.End.Hour, ev.End.Minute, 0);
                }

            }
            bdModel.SaveChanges();
            MessageBox.Show("Traitement terminé", "Plages horaires sauvegardées dans la BD");
            LoadScheduleSlotFromDatabase();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (NextPrevButtonVisibity)
            {
                this.btnNext.Visibility = Visibility.Visible;
                this.btnPrev.Visibility = Visibility.Visible;
            }
            // cbx populate
            cbxPeople.ItemsSource = bdModel.People.ToList();
            cbxPeople.SelectedValuePath = "idPerson";
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

        private void btnShowAllWorkingShifts_Click(object sender, RoutedEventArgs e)
        {
            cbxPeople.SelectedIndex = -1; //set to null
            PlanningGeneratorTools.ClearWorkingShiftScheduler(slotGenerationScheduler);
            LoadScheduleSlotFromDatabase();
        }
        private void cbxPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxPeople.SelectedIndex != -1)
            {
                Person pers = (Person)(cbxPeople.SelectedItem);
                AddAbsencePreferenceOnScheduler(pers);
            }
        }

        private void AddAbsencePreferenceOnScheduler(Person pers)
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

            PlanningGeneratorTools.ClearWorkingShiftScheduler(slotGenerationScheduler);
            List<AbsencePreference> scheduleAbsPreferencesSlotsWeek = bdModel.AbsencePreferences.Where(ws => ws.idPerson == pers.idPerson).ToList();
            //Get all the current week's scheduleSlots
            
            scheduleAbsPreferencesSlotsWeek = PlanningGeneratorTools.GetAbsPreferenceSlots(slotGenerationScheduler.SelectedDate, bdModel);
            scheduleAbsPreferencesSlotsWeek = scheduleAbsPreferencesSlotsWeek.Where(a => a.idPerson == pers.idPerson).ToList();

            //Add the week's slot in the scheduler
            foreach (AbsencePreference ss in scheduleAbsPreferencesSlotsWeek)
            {
                slotGenerationScheduler.AddEvent(new Event()
                {
                    Subject = ss.idPerson.ToString() + " " + ss.Person.firstName + " " + ss.Person.name + Environment.NewLine + Environment.NewLine +
                                "Date départ : " + ss.firstDay.ToString("MM/dd/yyyy") + Environment.NewLine + Environment.NewLine +
                                "Date de fin : " + ss.lastDate.ToString("MM/dd/yyyy"),
                    Start = lastSunday.AddDays(ss.dayOfWeek).Add(ss.startHour),
                    End = lastSunday.AddDays(ss.dayOfWeek).Add(ss.endHour),
                    DayOfWeek = ss.dayOfWeek,
                    Color = new SolidColorBrush(Colors.Aqua),
                    FirstDay = ss.firstDay,
                    LastDay = ss.lastDate,
                    IdShift = ss.idTimeSlot
                });
            }
        }
    }
}
