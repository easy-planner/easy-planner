﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfScheduler;

namespace EasyPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Scheduler scheduler = new Scheduler();
        }

        private void mainScheduler_Loaded(object s, RoutedEventArgs e)
        {
            bd_easyplannerEntities bdModel = new bd_easyplannerEntities();
            mainScheduler.SelectedDate = new DateTime(2016, 05, 17);
            mainScheduler.StartJourney = new TimeSpan(7, 0, 0);
            mainScheduler.EndJourney = new TimeSpan(19, 0, 0);
            mainScheduler.Loaded += mainScheduler_Loaded;
            mainScheduler.Mode = Mode.Week;
            foreach(WorkingShift ws in bdModel.WorkingShifts.ToList())
            {
                Person person = bdModel.People.Find(ws.idPerson);
                mainScheduler.AddEvent(new Event() {
                    Subject = person.firstName + " " + person.name,
                    Description = ws.description,
                    Color = colorPiker(person),
                    Start = (System.DateTime)ws.start,
                    End = (System.DateTime)ws.end,
                });
            }
        }

        private void prevBtn_Click(object s, RoutedEventArgs e)
        {
            mainScheduler.PrevPage();
        }

        private void nextBtn_Click(object s, RoutedEventArgs e)
        {
            mainScheduler.NextPage();
        }

        private void modeMonthBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Month;
        }

        private void modeWeekBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Week;
        }

        private void modeDayBtn_Click(object sender, RoutedEventArgs e)
        {
            mainScheduler.Mode = Mode.Day;
        }

        private Brush colorPiker(Person p)
        {
            return (Brush) new BrushConverter().ConvertFromString(typeof(System.Windows.Media.Brushes).GetProperties()[p.idPerson%80].Name);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window1 f1 = new Window1();
            f1.Show();
        }
    }
}
