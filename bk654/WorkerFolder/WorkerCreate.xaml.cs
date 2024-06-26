using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using bk654.Data;
using bk654.Models;
using System.Windows.Controls.Primitives;
using bk654.WorkerFolder;

namespace bk654
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private ApplicationContext dbContext;
        private List<Worker> _workers;
        public Window2()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Worker newWorker = new Worker
            {
                Surname = textBoxSurname.Text,
                Name = textBoxName.Text,
                Patronymic = textBoxPatronymic.Text,
                PositionId = int.Parse(positionWorker.Text),
                RestaurantId = int.Parse(restaurantWorker.Text),  
                StartDate = datePickerStartDate.SelectedDate ?? DateTime.Now,
                EndDate = datePickerEndDate.SelectedDate,
                DismissalReason = textBoxDismissalReason.Text
            };
            //ава
            dbContext.Workers.Add(newWorker);
            dbContext.SaveChanges();

            try
            {
                MessageBox.Show("Worker details saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
