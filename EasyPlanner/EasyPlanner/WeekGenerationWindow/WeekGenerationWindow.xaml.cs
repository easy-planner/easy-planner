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
        private List<WorkingShift> totalWorkingShifts = null;


        /// <summary>
        /// Construtor for the week generation Windows
        /// </summary>
        /// <param name="bd">Entity framework datbase</param>
        public WeekGenerationWindow(bd_easyplannerEntities bd)
        {
            InitializeComponent();

            this.bdModel = bd;
            totalWorkingShifts = new List<WorkingShift>();
            weekGenerationScheduler.SelectedDate = DateTime.Today;
            weekGenerationScheduler.StartJourney = new TimeSpan(7, 0, 0);
            weekGenerationScheduler.EndJourney = new TimeSpan(19, 0, 0);
            weekGenerationScheduler.Mode = Mode.Week;

            btnSaveInDB.IsEnabled = false;
            //weekGenerationScheduler.OnEventDoubleClick += weekGenerationScheduler_OnEventDoubleClick;
            //weekGenerationScheduler.OnScheduleDoubleClick += weekGenerationScheduler_OnScheduleDoubleClick;
        }

        //void weekGenerationScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        //{
        //    MessageBox.Show("Test PSI" + e.Date.ToShortDateString());
        //}

        //void weekGenerationScheduler_OnEventDoubleClick(object sender, Event e)
        //{
        //    MessageBox.Show("Test PSI" + e.IdShift);
        //}

        private void btnPrevWeekGeneration_Click(object sender, RoutedEventArgs e)
        {
            weekGenerationScheduler.PrevPage();
            dpFirstDay.SelectedDate = ((DateTime)dpFirstDay.SelectedDate).AddDays(-7);
            dpLastDay.SelectedDate = ((DateTime)dpLastDay.SelectedDate).AddDays(-7);
        }

        private void btnNextWeekGeneration_Click(object sender, RoutedEventArgs e)
        {
            weekGenerationScheduler.NextPage();
            dpFirstDay.SelectedDate = ((DateTime)dpFirstDay.SelectedDate).AddDays(7);
            dpLastDay.SelectedDate = ((DateTime)dpLastDay.SelectedDate).AddDays(7);
        }
        /// <summary>
        /// For selected week, call the method to generate the workingShifts.
        /// The workingShits are on the scheduler and ready to be persisted in db.
        /// </summary>
        private void btnGeneration_Click(object sender, RoutedEventArgs e)
        {
            if (isDateChecked() && areDatesCorrect())
            {
                MessageBox.Show("Le traitement peux prendre du temps merci de patienter jusqu'au message indicant la fin du traitement.", "Génération des plages horaires");
                // Disabled simulation's components
                dpFirstDay.IsEnabled = false;
                dpLastDay.IsEnabled = false;
                btnGeneration.IsEnabled = false;
                PlanningGeneratorTools.ClearWorkingShiftScheduler(weekGenerationScheduler);

                //gets first and last day
                DateTime firstDay = (DateTime)dpFirstDay.SelectedDate;
                DateTime lastDay = (DateTime)dpLastDay.SelectedDate;

                List<DateTime> generationDates = new List<DateTime>();
                List<WorkingShift> shifts = null;
                while (firstDay < lastDay)
                {
                    shifts = new FlowGraph(bdModel.People.ToList(), PlanningGeneratorTools.GetWeekScheduleSlots(firstDay, bdModel), firstDay).GetShifts();
                    PlanningGeneratorTools.AddWorkingShiftScheduler(shifts, weekGenerationScheduler);
                    totalWorkingShifts.AddRange(shifts);
                    firstDay=firstDay.AddDays(7); //go to the next week
                }
                MessageBox.Show("Traitement terminé", "Génération des plages horaires");
                btnSaveInDB.IsEnabled = true; //Enable the database recording button
            }
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
        }

        /// <summary>
        /// Save the generated shiftsWorking to the database.
        /// </summary>
        private void btnSaveInDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Le traitement peux prendre du temps merci de patienter jusqu'au message indicant la fin du traitement.", "Sauvgarde dans la base de données");

            if (totalWorkingShifts != null)
            {
                PlanningGeneratorTools.PersistWorkingShiftDataBase(totalWorkingShifts, bdModel);
                MessageBox.Show("Traitement terminé", "Plages horaires sauvegardées dans la BD");
                btnSaveInDB.IsEnabled = true;
            }

            btnSaveInDB.IsEnabled = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpFirstDay.SelectedDate = PlanningGeneratorTools.GetMondayBefore(DateTime.Today);
            dpLastDay.SelectedDate = PlanningGeneratorTools.GetSundayAfter(DateTime.Today);

        }
    }
}
