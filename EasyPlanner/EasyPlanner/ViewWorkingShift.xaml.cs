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
    /// Interaction logic for ViewWorkingShift.xaml
    /// </summary>
    public partial class ViewWorkingShift : Window
    {
        bd_easyplannerEntities bd = new bd_easyplannerEntities();
        public ViewWorkingShift()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //dg.ItemsSource = bd.ScheduleSlots.ToList();
           // dg.ItemsSource = bd.WorkingShifts.ToList();
        }

        private void btnWorkingShiftView_Click(object sender, RoutedEventArgs e)
        {
            dg.ItemsSource = bd.WorkingShifts.ToList();
        }

        
        WorkingShift workingShift;

        private void button_Click(object sender, RoutedEventArgs e)
        {


            workingShift = new WorkingShift();
            workingShift.start = DateTime.Now;
            workingShift.end = null;
            workingShift.description = "Test Plinio Sacchetti";
            workingShift.idPerson = 1;
            bd.WorkingShifts.Add(workingShift);
            bd.SaveChanges();
        }

        List<WorkingShift> wslist;
        List<ScheduleSlot> sllist;

        private void AddWorkingShiftOnScheduler(List<WorkingShift> wsList, Scheduler scheduler )
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            sllist = PlanningGeneratorTools.GetScheduleSlot(bd);
        }
    }
}
