using bk654.Data;
using Microsoft.EntityFrameworkCore;
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

namespace bk654.PositionAtWorkFolder
{
    /// <summary>
    /// Логика взаимодействия для UpdatePos.xaml
    /// </summary>
    public partial class UpdatePos : Window
    {
        private ApplicationContext dbContext;
        public UpdatePos()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int positionId = int.Parse(idOldPosition.Text);
                string newNameValue = newName.Text;
                decimal newSalaryValue = decimal.Parse(newSalary.Text);

                dbContext.Database.ExecuteSqlInterpolated($@"UPDATE position_at_work 
                                                     SET name = {newNameValue}, salary_per_hour = {newSalaryValue} 
                                                     WHERE position_id = {positionId}");

                MessageBox.Show("Position details updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
