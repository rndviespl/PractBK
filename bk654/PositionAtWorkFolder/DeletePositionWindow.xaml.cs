using bk654.Data;
using bk654.Models;
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
    /// Логика взаимодействия для DeletePositionWindow.xaml
    /// </summary>
    public partial class DeletePositionWindow : Window
    {
        private ApplicationContext dbContext;
        public DeletePositionWindow()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }

        private void DeletePositionById(int positionId)
        {
            PositionAtWork positionAtWork = dbContext.PositionAtWorks.FirstOrDefault(p => p.PositionId == positionId);
            if (positionAtWork != null) 
            { 
                dbContext.PositionAtWorks.Remove(positionAtWork);
                dbContext.SaveChanges();
                MessageBox.Show("должность успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("должность не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeletePositionButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(deletePositionTextBox.Text, out int positionId))
            {
                // Запрашиваем подтверждение перед удалением
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить должность?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeletePositionById(positionId);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный ID должности", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
