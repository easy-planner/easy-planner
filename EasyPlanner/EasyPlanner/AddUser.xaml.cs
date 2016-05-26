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
    /// Logique d'interaction pour AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        bd_easyplannerEntities bdModel;
        Person current;
        public AddUser(bd_easyplannerEntities bdModel)
        {
            InitializeComponent();
            this.bdModel = bdModel;
            cob_role.ItemsSource = bdModel.Roles.ToList();
            cob_role.DisplayMemberPath = "roleName";
            cob_role.SelectedValuePath = "idRole";
            
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            current = new Person();
            current.firstName = txt_firstname.Text;
            current.name = txt_name.Text;
            current.numberAVS = txtavs.Text;
            current.occupancyRate = float.Parse(txt_occupencyrate.Text);
            current.idRole = (int) cob_role.SelectedValue;
            bdModel.People.Add(current);
            bdModel.SaveChanges();
            this.Close();
        }
    }
}
