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
    public partial class Window1 : Window
    {
        bd_easyplannerEntities bd = new bd_easyplannerEntities();
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgTest.ItemsSource = bd.People.ToList();
        }

        private void cbDayOfWeek_DropDownClosed(object sender, EventArgs e)
        {
            MessageBox.Show("Jour de la semaine : " + cbDayOfWeek.Text);
            var original = bd.ScheduleSlots.SingleOrDefault(b => b.dayOfWeek == "lundi");
            if (original != null)
            {
               // original.dayOfWeek. = DayOfWeek.Saturday.;
            }
        }
    }
}
