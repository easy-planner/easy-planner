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


//Christophe Luthi
//Sprint 2
namespace EasyPlanner
{
    /// <summary>
    /// Logique d'interaction pour ManageUser.xaml
    /// </summary>
    public partial class ManageUser : Window
    {
        bd_easyplannerEntities bdModel;
        public ManageUser()
        {
            InitializeComponent();
            bdModel = bd_easyplannerEntities.OpenWithFallback();
            dgPeople.ItemsSource = bdModel.People.ToList();
        }


        private void dgPeople_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddUser auw = new AddUser((Person)((DataGrid)sender).SelectedItem, bdModel);
            auw.ShowDialog();

        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            AddUser auw = new AddUser((Person)(dgPeople.SelectedItem), bdModel);
            auw.ShowDialog();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bdModel.People.Remove((Person)dgPeople.SelectedItem);
            bdModel.SaveChanges();

        }
    }
}
