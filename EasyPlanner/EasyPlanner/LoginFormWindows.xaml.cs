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
    /// Logique d'interaction pour LoginFormWindow.xaml
    /// </summary>
    /// 
   
    public partial class LoginFormWindow : Window
    {
        bd_easyplannerEntities bdModel;
        Person user;
        Boolean isAdmin = false;

        public LoginFormWindow()
        {
            InitializeComponent();
            try {
                bdModel = bd_easyplannerEntities.OpenWithFallback();
            }
            catch (NoDataBaseUsableException e)
            {
                MessageBox.Show(e.Message);
                this.Close();
            }
/*            if (bdModel == null)
            {
                //afficher erreur
                this.Close();
            }*/
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text==""|| pwPassword.Password == "")
            {
                //erreur champ vide
            }
            else
            {
                List<Person> lsUser = bdModel.People.ToList();
                Person current;
                for(int i=0; i < lsUser.Count; i++)
                {
                    current = lsUser.ElementAt(i);
                    if(current.name.ToLower() == txtUserName.Text.ToLower() && current.password == pwPassword.Password)
                    {
                        user = current;
                    }
                }

                if(user != null)
                {
                    //ouvrir main
                    if(user.idRole == 3)
                    {
                        isAdmin = true;
                    }
                    new MainWindow(user, isAdmin).Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nom ou mot de passe incorrect", "Erreur de connexion");
                }
            }
        }
    }
}
