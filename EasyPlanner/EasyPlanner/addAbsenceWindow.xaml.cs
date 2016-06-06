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
    /// Interaction logic for addAbsenceWindow.xaml
    /// </summary>
    public partial class addAbsenceWindow : Window
    {
        private bd_easyplannerEntities bdModel;
        private Person persSelected;
        public addAbsenceWindow(Person p)
        {
            InitializeComponent();
            persSelected = p;
            bdModel = new bd_easyplannerEntities();

            lblPerson.Content += p.idPerson + " " + p.firstName + " " + p.name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dpFirstDay.SelectedDate = DateTime.Today;
            dpLastDay.SelectedDate = DateTime.Today;
            cbxEndHourHour.SelectedIndex = cbxEndHourHour.Items.Count-1;
            cbxEndHourMinute.SelectedIndex = cbxEndHourMinute.Items.Count-1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isDateChecked() && areDatesCorrect())
            {
                
                DateTime debut = (DateTime)dpFirstDay.SelectedDate;
                debut=debut.Add(new TimeSpan (Int32.Parse(cbxStartHourHour.Text), Int32.Parse(cbxStartHourMinute.Text), 0));

                DateTime fin = (DateTime)dpLastDay.SelectedDate;
                fin=fin.Add(new TimeSpan(Int32.Parse(cbxEndHourHour.Text), Int32.Parse(cbxEndHourMinute.Text), 0));

                List<WorkingShift> holidayWorkingShifts = bdModel.WorkingShifts.Where(ws => ws.idPerson == persSelected.idPerson && ws.start >= debut && ws.end <= fin).ToList();
                PlanningGeneratorTools.RemoveWorkingShiftDataBase(holidayWorkingShifts, bdModel);

                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez remplir les champs \"Premier jour\" et \"Dernier Jour\"" +
                    "\n" + " et vérifier que le premier jour soit avant ou égal au dernier jour");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Check if the first day and last day are set
        /// (if its not, then it will return false)
        /// </summary>
        /// <param>None</param>
        private Boolean isDateChecked()
        {
            Boolean isChecked = false;
            if (dpFirstDay.SelectedDate != null && dpLastDay.SelectedDate != null)
            {
                isChecked = true;
            }
            return isChecked;
        }

        /// <summary>
        /// Check if the first day is earlier or equals to the last day
        /// (if its not, then it will return false)
        /// </summary>
        /// <param>None</param>
        private Boolean areDatesCorrect()
        {
            Boolean areCorrect = false;
            if (dpFirstDay.SelectedDate <= dpLastDay.SelectedDate)
            {
                areCorrect = true;
            }
            return areCorrect;
        }

        private void dpFirstDay_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpLastDay.SelectedDate = dpFirstDay.SelectedDate;
        }
    }
}
