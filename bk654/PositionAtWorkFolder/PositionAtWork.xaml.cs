using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using bk654.Data;
using bk654.Models;
using System.Windows.Controls.Primitives;

namespace bk654
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ApplicationContext dbContext;
        private List<PositionAtWork> _positionAtWork;

        public Window1()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }

        private void ExecPos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = textBoxName.Text;
                decimal salary = decimal.Parse(textBoxSalary.Text);

                dbContext.Database.ExecuteSql($"INSERT INTO position_at_work (name, salary_per_hour) VALUES ({name}, {salary})");
                MessageBox.Show("должность успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}