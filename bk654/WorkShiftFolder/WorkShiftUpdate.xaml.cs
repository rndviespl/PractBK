using bk654.Data;
using bk654.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace bk654.WorkShiftFolder
{
    /// <summary>
    /// Логика взаимодействия для WorkShiftUpdate.xaml
    /// </summary>
    public partial class WorkShiftUpdate : Window
    {
        ApplicationContext dbContext;
        private WorkShift selectedWorkShift;

        public WorkShiftUpdate(WorkShift workShift)
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            selectedWorkShift = workShift;

            dateHourStartTextBox.Text = selectedWorkShift.StartShift.Hour.ToString();
            dateMinStartTextBox.Text = selectedWorkShift.StartShift.Minute.ToString();
            datePickerStartShift.SelectedDate = selectedWorkShift.StartShift.Date;

            dateHourEndTextBox.Text = selectedWorkShift.EndShift.Hour.ToString();
            dateMinEndTextBox.Text = selectedWorkShift.EndShift.Minute.ToString();
            textBoxDescriptionManualEntry.Text = selectedWorkShift.DescriptionManualEntry;
        }

        private void ApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            selectedWorkShift.StartShift = new DateTime(datePickerStartShift.SelectedDate.Value.Year, datePickerStartShift.SelectedDate.Value.Month,
                datePickerStartShift.SelectedDate.Value.Day, int.Parse(dateHourStartTextBox.Text), int.Parse(dateMinStartTextBox.Text), 0);

            selectedWorkShift.EndShift = new DateTime(datePickerStartShift.SelectedDate.Value.Year, datePickerStartShift.SelectedDate.Value.Month,
                datePickerStartShift.SelectedDate.Value.Day, int.Parse(dateHourEndTextBox.Text), int.Parse(dateMinEndTextBox.Text), 0);

            selectedWorkShift.DescriptionManualEntry = textBoxDescriptionManualEntry.Text;

            UpdateWorkShiftInDataSource(selectedWorkShift);

            MessageBox.Show("Запись успешно обновлена!");

            this.Close();
        }

        private void UpdateWorkShiftInDataSource(WorkShift workShift)
        {
            var existingWorkShift = dbContext.WorkShifts.FirstOrDefault(w => w.WorkShiftId == workShift.WorkShiftId);
            if (existingWorkShift != null)
            {
                existingWorkShift.StartShift = workShift.StartShift;
                existingWorkShift.EndShift = workShift.EndShift;
                existingWorkShift.DescriptionManualEntry = workShift.DescriptionManualEntry;
            }
            dbContext.SaveChanges();
        }
    }

}
