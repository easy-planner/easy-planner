/////////////////////////////////////////////////////////////
// Class responsible : Greg
// Last update : 30.05.2016
// Sprint 4 and 5
// Story(ies) : Create an view Schedule slots, Update and delete Scheduled slots
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

namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class ViewPlotsWindow : Window
    {
        bd_easyplannerEntities bdModel = bd_easyplannerEntities.OpenWithFallback();
        public ViewPlotsWindow()
        {
            InitializeComponent();
            dg.ItemsSource = bdModel.ScheduleSlots.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b.Name == "btnRemove")
            {
                //Look for the row in the database, in order to remove it.
                if ((dg.SelectedItem as ScheduleSlot).idTimeSlot != 0)
                {
                    int tsId = (dg.SelectedItem as ScheduleSlot).idTimeSlot;
                    ScheduleSlot sl = (from r in bdModel.ScheduleSlots where r.idTimeSlot == tsId select r).SingleOrDefault();
                    bdModel.ScheduleSlots.Remove(sl);
                    bdModel.SaveChanges();
                    dg.ItemsSource = bdModel.ScheduleSlots.ToList();
                }


            } else if (b.Name == "btnUpdate")
            {
                int tsId = (dg.SelectedItem as ScheduleSlot).idTimeSlot;
                ScheduleSlot s1 = (from r in bdModel.ScheduleSlots where r.idTimeSlot == tsId select r).SingleOrDefault();
                ScheduleSlot tmp = s1;
                
                AddPlotWindow apw = new AddPlotWindow(tmp, bdModel);
                //bd.ScheduleSlots.Remove(s1);
                apw.ShowDialog();
                
                bdModel.SaveChanges();
                dg.ItemsSource = bdModel.ScheduleSlots.ToList();

                
            } 


        }
    }
}
