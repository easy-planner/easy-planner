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
            cbxRole.ItemsSource = bdModel.Roles.ToList();
            cbxRole.DisplayMemberPath = "roleName";
            cbxRole.SelectedValuePath = "idRole";
            modify = false;
        }

        public AddUser(Person current, bd_easyplannerEntities bd)
        {
            InitializeComponent();

            bdModel = bd;
            modify = true;
            this.current = current;
            txtName.Text = current.name;
            txtFirstname.Text = current.firstName;
            txtAvs.Text = current.numberAVS;
            txtOccupencyrate.Text = current.occupancyRate.ToString();
            txtDescription.Text = current.description;
            cbxRole.ItemsSource = bdModel.Roles.ToList();
            cbxRole.DisplayMemberPath = "roleName";
            cbxRole.SelectedValuePath = "idRole";
            cbxRole.SelectedValue = current.idRole;
            btnSave.Content = "Modifier";
            
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!modify)
            {
                current = new Person();
                current.password = pwdPassword.Password;
            }else if (pwdPassword.Password != null)
            {
                current.password = pwdPassword.Password;
            }
           
            current.firstName = txtFirstname.Text;
            current.name = txtName.Text;
            current.numberAVS = txtAvs.Text;
            current.occupancyRate = float.Parse(txtOccupencyrate.Text);
            current.idRole = (int)cbxRole.SelectedValue;
            
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
