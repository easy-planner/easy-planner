using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfScheduler;

namespace EasyPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bd_easyplannerEntities bdModel;
        public MainWindow()
        {
            InitializeComponent();
            bdModel = new bd_easyplannerEntities();

            mainScheduler.OnEventDoubleClick += MainScheduler_OnEventDoubleClick;
            mainScheduler.OnScheduleDoubleClick += MainScheduler_OnScheduleDoubleClick;
        }
        void MainScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            MessageBox.Show("Création d'une tranche horaire à implémenter...","Implémentation non terminée." + e.Date.ToShortDateString());
        }

        void MainScheduler_OnEventDoubleClick(object sender, Event e)
        {
            MessageBox.Show("Modification ou suppression de la tranche horaire sélectionnée à implémenter...", "Implémentation non terminée." + e.Id.ToString());
        }

        private void mainScheduler_Loaded(object s, RoutedEventArgs e)
        {
            mainScheduler.SelectedDate = DateTime.Today;
            mainScheduler.StartJourney = new TimeSpan(7, 0, 0);
            mainScheduler.EndJourney = new TimeSpan(19, 0, 0);
            mainScheduler.Loaded += mainScheduler_Loaded;
            mainScheduler.Mode = Mode.Week;
            updateEvents();
        }

        private void updateEvents()
        {
            foreach (WorkingShift ws in bdModel.WorkingShifts.ToList())
            {
                Person person = bdModel.People.Find(ws.idPerson);
                mainScheduler.AddEvent(new Event()
                {
                    Subject = ws.idShift + "-" + Environment.NewLine + person.firstName + " " + person.name,
                    Description = ws.description,
                    Color = colorPiker(person),
                    Start = (System.DateTime)ws.start,
                    End = (System.DateTime)ws.end,
                    IdShift = ws.idShift,
                });
            }
        }

        private void mnViewUsers(object s, RoutedEventArgs e)
        {
            ManageUser viewUsersWindow = new ManageUser();
            viewUsersWindow.ShowDialog();
        }

        private void mnAddUser(object s, RoutedEventArgs e)
        {
            AddUser addUserWindow = new AddUser();
            addUserWindow.ShowDialog();
        }

        private void mnViewPlots(object s, RoutedEventArgs e)
        {
            ViewPlotsWindow viewPlotsWindow = new ViewPlotsWindow();
            viewPlotsWindow.ShowDialog();
        }

        private void mnAddPlot(object s, RoutedEventArgs e)
        {
            AddPlotWindow addPlotWindow = new AddPlotWindow();
            addPlotWindow.ShowDialog();
        }

        private void btnPrev_Click(object s, RoutedEventArgs e)
        {
            mainScheduler.PrevPage();
        }

        private void btnNext_Click(object s, RoutedEventArgs e)
        {
            mainScheduler.NextPage();
        }

        private void modeMonthBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Month;
        }

        private void modeWeekBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Week;
        }

        private void modeDayBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Day;
        }

        private Brush colorPiker(Person p)
        {
            return (Brush) new BrushConverter().ConvertFromString(typeof(Brushes).GetProperties()[p.idPerson%(typeof(Brushes).GetProperties().Count())].Name);
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            updateEvents();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AddPlotWindow apw = new AddPlotWindow();
            apw.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ViewPlotsWindow vpw = new ViewPlotsWindow();
            vpw.Show();
        }

        /// <summary>
        /// Open a new window for the workingshifts generation.
        /// When it is finished, all workinshifts are in database.
        /// </summary>
        private void mnWeeksGeneration(object sender, RoutedEventArgs e)
        {
            WeekGenerationWindow weekGenerate = new WeekGenerationWindow(bdModel);
            weekGenerate.ShowDialog();
            PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
            updateEvents();
        }

        /// <summary>
        /// Open the window to genrerate workingShifts.
        /// </summary>
        private void mnScheduleSlotsGeneration(object sender, RoutedEventArgs e)
        {
            SlotGenerationWindow slotsGenerate = new SlotGenerationWindow(bdModel);
            slotsGenerate.ShowDialog();
        }
        /// <summary>
        /// Open the windows to manage existing ScheduleSlot
        /// </summary>
        private void mnScheduleSlotsInScheduler(object sender, RoutedEventArgs e)
        {
            SlotGenerationWindow slotsGenerate = new SlotGenerationWindow(bdModel);
            slotsGenerate.LoadScheduleSlotFromDatabase();
            slotsGenerate.NextPrevButtonVisibity = true;
            slotsGenerate.ShowDialog();
        }

        /// <summary>
        /// Occurs when window is loaded. Initialize components
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // cbx populate
            cbxPeople.ItemsSource = bdModel.People.ToList();
            cbxPeople.SelectedValuePath = "idPerson";

            btnAbsence.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Showing selected person workingShifts in the scheduler
        /// </summary>
        private void cbxPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnAbsence.Visibility = Visibility.Hidden;
            if (cbxPeople.SelectedIndex != -1)
            {
                PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
                List<WorkingShift> personnalWorkingShifts = bdModel.WorkingShifts.Where(ws => ws.idPerson == (int)cbxPeople.SelectedValue).ToList();
                PlanningGeneratorTools.AddWorkingShiftScheduler(personnalWorkingShifts, mainScheduler);
                btnAbsence.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Showing all people workingShifts in the scheduler
        /// </summary>
        private void btnShowAllWorkingShifts_Click(object sender, RoutedEventArgs e)
        {
            cbxPeople.SelectedIndex = -1; //set to null
            PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
            PlanningGeneratorTools.AddWorkingShiftScheduler(bdModel.WorkingShifts.ToList(), mainScheduler);
        }

        /// <summary>
        /// Remove all workingshifts from database and scheduler
        /// </summary>
        private void mnWorkingShiftsRemove(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Confirmez-vous la suppression de toutes les tranches horaires des employés?", "Suppression tranches horaires", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                List<WorkingShift> allWorkingShifts = bdModel.WorkingShifts.ToList();
                PlanningGeneratorTools.RemoveWorkingShiftDataBase(allWorkingShifts, bdModel);
                PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
                MessageBox.Show("Traitement terminé", "Plages horaires supprimées dans la BD");
            }
        }

        private void btnAbsence_Click(object sender, RoutedEventArgs e)
        {
            addAbsenceWindow absWin = new addAbsenceWindow((Person)cbxPeople.SelectedItem);
            absWin.ShowDialog();
            PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
            cbxPeople.SelectedIndex = -1;
            updateEvents();
        }

        private void mnScheduleAbsencePreference(object sender, RoutedEventArgs e)
        {
            AbsencePreferenceGenerationWindow absPrefSlot = new AbsencePreferenceGenerationWindow(bdModel);
            absPrefSlot.LoadScheduleSlotFromDatabase();
            absPrefSlot.NextPrevButtonVisibity = false;
            absPrefSlot.ShowDialog();
        }
    }
}
