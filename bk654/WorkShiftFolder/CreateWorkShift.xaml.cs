using bk654.Data;
using bk654.Models;
using System.Windows;

namespace bk654
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        private ApplicationContext dbContext;
        private List<WorkShift> _workShifts;
        public Window3()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
        }

        private void CreateWorkShift_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxWorkerId.Text, out int workerId)
       && int.TryParse(dateHourStartTextBox.Text, out int hourStart)
       && int.TryParse(dateMinStartTextBox.Text, out int minStart)
       && int.TryParse(dateHourEndTextBox.Text, out int hourEnd)
       && int.TryParse(dateMinEndTextBox.Text, out int minEnd)
       && DateTime.TryParse(datePickerStartShift.Text, out DateTime startDate))
            {
                DateTime startShift = new(startDate.Year, startDate.Month, startDate.Day, hourStart, minStart, 0);
                DateTime endShift = new(startDate.Year, startDate.Month, startDate.Day, hourEnd, minEnd, 0);

                WorkShift newWorkShift = new()
                {
                    WorkerId = workerId,
                    StartShift = startShift,
                    EndShift = endShift,
                    DescriptionManualEntry = textBoxDescriptionManualEntry.Text
                };

                dbContext.WorkShifts.Add(newWorkShift);
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
}

