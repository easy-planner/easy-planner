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
            Button b = sender as Button;
            if (b.Name == "btnRemove")
            {
                if ((dg.SelectedItem as ScheduleSlot).idTimeSlot != 0)
                {
                    int tsId = (dg.SelectedItem as ScheduleSlot).idTimeSlot;
                    ScheduleSlot sl = (from r in bd.ScheduleSlots where r.idTimeSlot == tsId select r).SingleOrDefault();
                    bd.ScheduleSlots.Remove(sl);
                    bd.SaveChanges();
                    dg.ItemsSource = bd.ScheduleSlots.ToList();
                }


            } else if (b.Name == "btnUpdate")
            {
                int tsId = (dg.SelectedItem as ScheduleSlot).idTimeSlot;
                ScheduleSlot s1 = (from r in bd.ScheduleSlots where r.idTimeSlot == tsId select r).SingleOrDefault();
                ScheduleSlot tmp = s1;
                
                AddPlotWindow apw = new AddPlotWindow(tmp);
                bd.ScheduleSlots.Remove(s1);
                apw.ShowDialog();
                bd.SaveChanges();
                dg.ItemsSource = bd.ScheduleSlots.ToList();
            } 


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
