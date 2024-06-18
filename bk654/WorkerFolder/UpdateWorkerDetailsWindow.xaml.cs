using bk654.Models;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace bk654.WorkerFolder
{
    public partial class UpdateWorkerDetailsWindow : Window
    {
        private Worker _worker;

        public UpdateWorkerDetailsWindow(Worker worker)
        { 
            InitializeComponent();
            _worker = worker;

            Loaded += UpdateWorkerDetailsWindow_Loaded; // Подписываемся на событие Loaded
        }
        private void UpdateWorkerDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Заполняем поля ввода данными о работнике при загрузке окна
            textBoxSurname.Text = _worker.Surname;
            textBoxName.Text = _worker.Name;
            textBoxPatronymic.Text = _worker.Patronymic;
            positionWorker.Text = _worker.PositionId.ToString();
            restaurantWorker.Text = _worker.RestaurantId.ToString();
            datePickerStartDate.SelectedDate = _worker.StartDate;
            datePickerEndDate.SelectedDate = _worker.EndDate;
            textBoxDismissalReason.Text = _worker.DismissalReason;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные работника на основе введенных данных в окне
            _worker.Surname = textBoxSurname.Text;
            _worker.Name = textBoxName.Text;
            _worker.Patronymic = textBoxPatronymic.Text;
            _worker.PositionId = int.Parse(positionWorker.Text);
            _worker.RestaurantId = int.Parse(restaurantWorker.Text);
            _worker.StartDate = datePickerStartDate.SelectedDate ?? DateTime.Now;
            _worker.EndDate = datePickerEndDate.SelectedDate;
            _worker.DismissalReason = textBoxDismissalReason.Text;

            // Пометим окно как успешное и закроем его
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Пометим окно как отмененное и закроем его
            this.DialogResult = false;
            this.Close();
        }
    }
}