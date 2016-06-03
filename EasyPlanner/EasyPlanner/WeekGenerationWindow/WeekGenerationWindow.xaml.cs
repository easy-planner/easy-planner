/////////////////////////////////////////////////////////////
// Class responsible : Plinio Sacchetti
// Last update : 03.06.2016
// Sprint 5
// Story(ies) : Generate a scheduler
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
    public partial class WeekGenerationWindow : Window
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
        public WeekGenerationWindow(bd_easyplannerEntities bd)
        {
            InitializeComponent();

            this.bdModel = bd;
            weekGenerationScheduler.SelectedDate = DateTime.Today;
            weekGenerationScheduler.StartJourney = new TimeSpan(7, 0, 0);
            weekGenerationScheduler.EndJourney = new TimeSpan(19, 0, 0);
            weekGenerationScheduler.Mode = Mode.Week;

            weekGenerationScheduler.OnEventDoubleClick += weekGenerationScheduler_OnEventDoubleClick;
            weekGenerationScheduler.OnScheduleDoubleClick += weekGenerationScheduler_OnScheduleDoubleClick;
        }

        void weekGenerationScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            MessageBox.Show("Test PSI" + e.Date.ToShortDateString());
        }

        void weekGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        {
            MessageBox.Show("Test PSI" + e.IdShift);
        }

        private void btnPrevWeekGeneration_Click(object sender, RoutedEventArgs e)
        {
            weekGenerationScheduler.PrevPage();
        }

        private void btnNextWeekGeneration_Click(object sender, RoutedEventArgs e)
        {
            weekGenerationScheduler.NextPage();
        }
        private void btnGeneration_Click(object sender, RoutedEventArgs e)
        {
            if (isDateChecked() && areDatesCorrect())
            {
                PlanningGeneratorTools.ClearWorkingShiftScheduler(weekGenerationScheduler);

                //gets first and last day
                DateTime firstDay = (DateTime)dpFirstDay.SelectedDate;
                DateTime lastDay = (DateTime)dpLastDay.SelectedDate;


                List<DateTime> generationDates = new List<DateTime>();
                List<WorkingShift> shifts;
                while (firstDay < lastDay)
                {
                    shifts = new FlowGraph(bdModel.People.ToList(), PlanningGeneratorTools.GetWeekScheduleSlots(firstDay, bdModel), firstDay).GetShifts();
                    PlanningGeneratorTools.AddWorkingShiftScheduler(shifts, weekGenerationScheduler);
                    firstDay=firstDay.AddDays(7); //go to the next week
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }

        }
    }
}
