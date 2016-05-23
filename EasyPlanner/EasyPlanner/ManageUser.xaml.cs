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
    /// Logique d'interaction pour ManageUser.xaml
    /// </summary>
    public partial class ManageUser : Window
    {
        bd_easyplannerEntities bd;
        public ManageUser(bd_easyplannerEntities bdModel)
        {
            InitializeComponent();
            bd = bdModel;
            dgPeople.ItemsSource = bd.People.ToList();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AddUser wau = new AddUser(bd);
            wau.Show(); 
        }
    }
}
