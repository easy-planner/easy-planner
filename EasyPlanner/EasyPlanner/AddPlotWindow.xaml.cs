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
    public partial class AddPlotWindow : Window
    {
        bd_easyplannerEntities bd = new bd_easyplannerEntities();
        private ScheduleSlot current;

        public AddPlotWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //dgTest.ItemsSource = bd.People.ToList();
            //dgTest.ItemsSource = bd.ScheduleSlots.ToList();
        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            current = new ScheduleSlot();
            current.dayOfWeek = cbDayOfWeek.Text;
            current.startHour = new TimeSpan(Int32.Parse(cbStartHourHour.Text), Int32.Parse(cbStartHourMinute.Text), 0);
            current.endHour = new TimeSpan(Int32.Parse(cbEndHourHour.Text), Int32.Parse(cbEndHourMinute.Text), 0);
            current.minAttendency = Int32.Parse(cbAttendency.Text);
            current.firstDay = dpFirstDay.SelectedDate;
            current.lastDay = dpLastDay.SelectedDate;
            bd.ScheduleSlots.Add(current);
            bd.SaveChanges();
            this.Close();
            }
    }
}
