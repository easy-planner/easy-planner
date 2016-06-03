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


        /// <summary>
        /// Construtor for the week generation Windows
        /// </summary>
        /// <param name="bd">Entity framework datbase</param>
        public SlotGenerationWindow(bd_easyplannerEntities bd)
        {
            InitializeComponent();
            this.bdModel = bd;
            bdModel = new bd_easyplannerEntities();

            //Scheduler settings
            slotGenerationScheduler.SelectedDate = DateTime.Today;
            slotGenerationScheduler.StartJourney = new TimeSpan(7, 0, 0);
            slotGenerationScheduler.EndJourney = new TimeSpan(19, 0, 0);
            slotGenerationScheduler.Mode = Mode.Week;

            //Activate double-click on the scheduler
            slotGenerationScheduler.OnEventDoubleClick += slotGenerationScheduler_OnEventDoubleClick;
            slotGenerationScheduler.OnScheduleDoubleClick += slotGenerationScheduler_OnScheduleDoubleClick;
        }

        void slotGenerationScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
            slotScheduleWin.ShowDialog();
        }

        void slotGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        {
            AddSlotScheduleWindow slotScheduleWin = new AddSlotScheduleWindow(e, slotGenerationScheduler);
            slotScheduleWin.ShowDialog();
        }

        private void btnSlotScheduleGeneration_Click(object sender, RoutedEventArgs e)
        {
            if (isDateChecked() && areDatesCorrect())
            {
                //PlanningGeneratorTools.ClearWorkingShiftScheduler(slotGenerationScheduler);

                //gets first and last day
                DateTime firstDaySelection = (DateTime)dpFirstDay.SelectedDate;
                DateTime lastDaySelection = (DateTime)dpLastDay.SelectedDate;

                foreach (Event ev in slotGenerationScheduler.Events)
                {
                    bdModel.ScheduleSlots.Add(new ScheduleSlot()
                    {
                        dayOfWeek = ev.DayOfWeek,
                        firstDay = firstDaySelection,
                        lastDay = lastDaySelection,
                        minAttendency = ev.MinAttendency,
                        startHour = new TimeSpan(ev.Start.Hour, ev.Start.Minute,0),
                        endHour = new TimeSpan(ev.End.Hour, ev.End.Minute, 0),
                    });
                }
                bdModel.SaveChanges();
            }
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
        }
    }
}
