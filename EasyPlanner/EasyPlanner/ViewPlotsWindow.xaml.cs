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

namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class ViewPlotsWindow : Window
    {
        bd_easyplannerEntities bd = new bd_easyplannerEntities();
        public ViewPlotsWindow()
        {
            InitializeComponent();
            dg.ItemsSource = bd.ScheduleSlots.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int tsId = (dg.SelectedItem as ScheduleSlot).idTimeSlot;
            ScheduleSlot sl = (from r in bd.ScheduleSlots where r.idTimeSlot == tsId select r).SingleOrDefault();
            bd.ScheduleSlots.Remove(sl);
            bd.SaveChanges();
            dg.ItemsSource = bd.ScheduleSlots.ToList();
        }
    }
}
