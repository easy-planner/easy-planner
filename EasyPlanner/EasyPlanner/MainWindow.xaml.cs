using System;
using System.Collections.Generic;
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

            //Plinio (PSI)
            mainScheduler.OnEventDoubleClick += MainScheduler_OnEventDoubleClick;
            mainScheduler.OnScheduleDoubleClick += MainScheduler_OnScheduleDoubleClick;
        }
        void MainScheduler_OnScheduleDoubleClick(object sender, DateTime e)
        {
            MessageBox.Show("Test PSI" + e.Date.ToShortDateString());
        }

        void MainScheduler_OnEventDoubleClick(object sender, Event e)
        {
            MessageBox.Show("Test PSI" + e.IdShift);
        }

        private void mainScheduler_Loaded(object s, RoutedEventArgs e)
        {
            //mainScheduler.SelectedDate = new DateTime(2016, 05, 17);
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
                    //Subject = person.firstName + " " + person.name,
                    // Modif pour test plinio (psi)
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



        private void prevBtn_Click(object s, RoutedEventArgs e)
        {
            mainScheduler.PrevPage();
        }

        private void nextBtn_Click(object s, RoutedEventArgs e)
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

        private void addEventBtn_Click(object sender, RoutedEventArgs e)
        {
            List<WorkingShift> ws = new List<WorkingShift>();
            DateTime todayStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+7, 5, 00, 00);
            DateTime todayEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+7, 7, 20, 00);

            for (int i = 0; i < 5; i++)
            {
                ws.Add(new WorkingShift
                {
                    description = "Psi" + i + 1,
                    idShift = i + 1,
                    Person = bdModel.People.First(p => p.idPerson == 5),
                    start = todayStart.AddDays(i),
                    end = todayEnd.AddDays(i),
                });
            }

            PlanningGeneratorTools.AddWorkingShiftScheduler(ws, mainScheduler);
            //PlanningGeneratorTools.PersistWorkingShiftDataBase(ws, bdModel);
            //PlanningGeneratorTools.RemoveWorkingShiftDataBase(ws, bdModel);

        }

        private void clearEventBtn_Click(object sender, RoutedEventArgs e)
        {
            PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
        }

        private void addDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            List<WorkingShift> ws = new List<WorkingShift>();
            DateTime todayStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 7, 5, 00, 00);
            DateTime todayEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 7, 7, 20, 00);

            for (int i = 0; i < 5; i++)
            {
                ws.Add(new WorkingShift
                {
                    description = "Psi" + i + 1,
                    idShift = i + 1,
                    Person = bdModel.People.First(p => p.idPerson == 5),
                    start = todayStart.AddDays(i),
                    end = todayEnd.AddDays(i),
                });
            }

            PlanningGeneratorTools.AddWorkingShiftScheduler(ws, mainScheduler);
            PlanningGeneratorTools.PersistWorkingShiftDataBase(ws, bdModel);
        }

        private void removeDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            List<WorkingShift> ws = new List<WorkingShift>();
            for (int i = int.Parse(removeFirstTbx.Text); i <= int.Parse(removeLastTbx.Text); i++)
            {
                ws.Add(new WorkingShift
                {
                    idShift = i,
                });
            }

            PlanningGeneratorTools.RemoveWorkingShiftDataBase(ws, bdModel);
            PlanningGeneratorTools.RemoveWorkingShiftScheduler(ws, mainScheduler);
        }

        private void slotsShiftsInWeekBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime d = mainScheduler.SelectedDate;

            String s = "Semaine comprenant le " + d.ToString() + Environment.NewLine;
            s += "========================================== " + Environment.NewLine + Environment.NewLine;
            List<ScheduleSlot> scheduleSlotsWeek;

            scheduleSlotsWeek = PlanningGeneratorTools.GetWeekScheduleSlots(d, bdModel);
            foreach (ScheduleSlot slot in scheduleSlotsWeek)
            {
                s += slot.idTimeSlot + " - " 
                    + slot.dayOfWeek + "  de " + slot.startHour + " à " + slot.endHour
                    + " du " + String.Format("{0:MM/dd/yyyy}", slot.firstDay) + " au " + String.Format("{0:MM/dd/yyyy}", slot.lastDay)
                    + " , nb min : " + slot.minAttendency;
                s += Environment.NewLine;
            }
            MessageBox.Show(s);
        }

        private void testGenerationBtn_Click(object sender, RoutedEventArgs e)
        {
            List<WorkingShift> shifts = new FlowGraph(bdModel.People.ToList(), PlanningGeneratorTools.GetWeekScheduleSlots(mainScheduler.SelectedDate,bdModel), mainScheduler.SelectedDate).GetShifts();
            PlanningGeneratorTools.ClearWorkingShiftScheduler(mainScheduler);
            PlanningGeneratorTools.AddWorkingShiftScheduler(shifts, mainScheduler);
        }
    }
}
