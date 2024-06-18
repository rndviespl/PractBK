using bk654.Data;
using bk654.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace bk654.WorkerFolder
{
    public partial class UpdateWorker : Window
    {
        private ApplicationContext dbContext;
        private List<Worker> _workers;
        public UpdateWorker()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }

        private void ChangeWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем id работника из текстового поля
            int workerId = int.Parse(idOldWorker.Text);

            // Получаем данные работника из базы данных по его id
            Worker workerToUpdate = dbContext.Workers.FirstOrDefault(w => w.WorkerId == workerId);

            if (workerToUpdate != null)
            {
                // Создаем новое окно для обновления данных работника
                UpdateWorkerDetailsWindow updateWorkerWindow = new UpdateWorkerDetailsWindow(workerToUpdate);
                updateWorkerWindow.ShowDialog(); // Показываем новое окно как диалоговое окно

                // После закрытия окна обновления данных проверяем результат
                if (updateWorkerWindow.DialogResult == true) 
                {
                    dbContext.SaveChanges();

                    MessageBox.Show("Данные работника успешно обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Отмена обновления данных работника", "Отмена", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Сотрудник с указанным ID не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}