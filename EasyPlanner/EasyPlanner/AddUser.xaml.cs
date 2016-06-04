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
        Boolean modify;
        public AddUser()
        {
            InitializeComponent();
            bdModel = new bd_easyplannerEntities();
            cob_role.ItemsSource = bdModel.Roles.ToList();
            cob_role.DisplayMemberPath = "roleName";
            cob_role.SelectedValuePath = "idRole";
            modify = false;
        }

        public AddUser(Person current, bd_easyplannerEntities bd)
        {
            InitializeComponent();

            bdModel = bd;
            modify = true;
            this.current = current;
            txtName.Text = current.name;
            txt_firstname.Text = current.firstName;
            txtavs.Text = current.numberAVS;
            txt_occupencyrate.Text = current.occupancyRate.ToString();
            txtDescription.Text = current.description;
            cob_role.ItemsSource = bdModel.Roles.ToList();
            cob_role.DisplayMemberPath = "roleName";
            cob_role.SelectedValuePath = "idRole";
            cob_role.SelectedValue = current.idRole;
            btn_save.Content = "Modifier";
            
        }


        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (!modify)
            {
                current = new Person();
                current.password = pwdPassword.Password;
            }else if (pwdPassword.Password != null)
            {
                current.password = pwdPassword.Password;
            }
           
            current.firstName = txt_firstname.Text;
            current.name = txtName.Text;
            current.numberAVS = txtavs.Text;
            current.occupancyRate = float.Parse(txt_occupencyrate.Text);
            current.idRole = (int)cob_role.SelectedValue;
            
            current.description = txtDescription.Text;
            if (!modify)
            {
                bdModel.People.Add(current);
            }
            bdModel.SaveChanges();
            this.Close();
        }

    }
}
