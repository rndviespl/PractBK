using bk654.Data;
using bk654.Models;
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

namespace bk654.WorkerFolder
{
    /// <summary>
    /// Логика взаимодействия для DeleteWorkerWindow.xaml
    /// </summary>
    public partial class DeleteWorkerWindow : Window
    {
        private ApplicationContext dbContext;
        public DeleteWorkerWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }
        private void DeleteWorkerById(int workerId)
        {
            // Находим работника в базе данных по ID
            Worker workerToDelete = dbContext.Workers.FirstOrDefault(w => w.WorkerId == workerId);

            if (workerToDelete != null)
            {
                dbContext.Workers.Remove(workerToDelete);
                dbContext.SaveChanges();

                MessageBox.Show("Работник успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Работник не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteWorkerByIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxWorkerId.Text, out int workerId))
            {
                // Запрашиваем подтверждение перед удалением
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить работника?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteWorkerById(workerId);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный ID работника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
