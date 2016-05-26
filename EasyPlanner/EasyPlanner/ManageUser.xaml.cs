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
        bd_easyplannerEntities bdModel;
        public ManageUser()
        {
            InitializeComponent();
            bdModel = new bd_easyplannerEntities();
            dgPeople.ItemsSource = bdModel.People.ToList();
        }

    }
}
