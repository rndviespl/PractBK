using bk654.Data;
using bk654.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace bk654.WorkerFolder
{
    public partial class ViewWorker : Window
    {
        private ApplicationContext dbContext;
        private List<Worker> _workers;
        private List<WorkShift> _workShifts;
        private List<PerformanceReview> _performanceReviews;
        private int _currentPage = 1;
        private int _pageSize;
        private int _totalPages;

        public ViewWorker()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadDataWorker(_currentPage, "worker_id");
        }

        public void LoadDataWorker(int pageNumber, string sortBy, string searchCriteria = "")
        {
            try
            {
                IQueryable<Worker> query = dbContext.Workers;

                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    var keywords = searchCriteria.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var keyword in keywords)
                    {
                        query = query.Where(w => w.Surname.ToLower().Contains(keyword) || w.Name.ToLower().Contains(keyword));
                    }
                }
                int totalRecords = query.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
                int offset = (pageNumber - 1) * _pageSize;

                _workers = query.OrderBy(w => w.WorkerId).Skip(offset).Take(_pageSize).ToList();
                dataGrid.ItemsSource = _workers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadDataWorker(_currentPage, "worker_id");
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            LoadDataWorker(_currentPage, "worker_id");
        }
        private void ApplyRecordsPerPage_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(recordsPerPageTextBox.Text, out int recordsPerPage))
            {
                _pageSize = recordsPerPage;
                _currentPage = 1;
                LoadDataWorker(_currentPage, "worker_id");
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            LoadDataWorker(_currentPage, "worker_id");
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _totalPages;
            LoadDataWorker(_currentPage, "worker_id");
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = searchTextBox.Text.Replace("*", "").Trim();
            _currentPage = 1;
            LoadDataWorker(_currentPage, "worker_id", searchCriteria);
        }
        private void ShowWorkedHours_Click(object sender, RoutedEventArgs e)
        {
            var selectedWorker = (Worker)dataGrid.SelectedItem;
            List<WorkShift> shiftsForSelectedWorker = dbContext.WorkShifts.OrderBy(ws => ws.WorkerId).Where(ws => ws.WorkerId == selectedWorker.WorkerId).ToList();
            List<PositionAtWork> positions = dbContext.PositionAtWorks.ToList(); // Получаем список должностей

            ShowWorkedHoursForWorker(selectedWorker, shiftsForSelectedWorker, positions);
        }

        private void ShowWorkedHoursForWorker(Worker worker1, List<WorkShift> workShifts, List<PositionAtWork> positions)
        {
            var halfMonthShifts = workShifts.GroupBy(ws => new { Year = ws.StartShift.Year, Month = ws.StartShift.Month, Half = ws.StartShift.Day <= 15 ? "Первая" : "Вторая" })
                                            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month).ThenBy(g => g.Key.Half)
                                            .Select(group => new
                                            {
                                                Year = group.Key.Year,
                                                Month = group.Key.Month,
                                                Half = group.Key.Half,
                                                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month),
                                                TotalHours = Math.Round(group.Sum(ws => (ws.EndShift - ws.StartShift).TotalHours), 2),
                                                Salary = ((positions.FirstOrDefault(p => p.PositionId == worker1.PositionId)?.SalaryPerHour ?? 0) *
                                                (decimal)Math.Round(group.Sum(ws => (ws.EndShift - ws.StartShift).TotalHours), 2))
                                            }).ToList();

            DataGrid workedHoursDataGrid = new DataGrid();
            workedHoursDataGrid.ItemsSource = halfMonthShifts;

            Window workedHoursWindow = new Window();
            workedHoursWindow.Title = $"Отработанные часы для {worker1.Name} {worker1.Surname} {worker1.Patronymic} (Сотрудник ID: {worker1.WorkerId}, Ресторан ID: {worker1.RestaurantId})";
            workedHoursWindow.Content = workedHoursDataGrid;
            workedHoursWindow.ShowDialog();
        }

        private void ShowFormButton_Click(object sender, RoutedEventArgs e)
        {

            var selectedWorker = (Worker)dataGrid.SelectedItem;
            if (selectedWorker != null)
            {
                var selectedReview = dbContext.PerformanceReviewSummaries.FirstOrDefault(w => w.WorkerId == selectedWorker.WorkerId);
                if (selectedReview != null)
                {
                    ShowWorkedReviewSummary(selectedReview, selectedWorker);
                }
                else
                {
                    MessageBox.Show("Отзывы о выбранном сотруднике не найдены.", "Отсутствует информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void ShowWorkedReviewSummary(PerformanceReviewSummary selectedWorker, Worker worker1)
        {
            try
            {
                var performanceSummary = dbContext.PerformanceReviewSummaries.Where(summary => summary.WorkerId == selectedWorker.WorkerId).Select(summary => new
                {
                    WorkerId = summary.WorkerId,
                    TotalRating = summary.AvgRating != null ? Math.Round(summary.AvgRating.Value, 2) : 0,
                    MaxRating = summary.MaxRating ?? 0,
                    MinRating = summary.MinRating ?? 0
                }).ToList();

                    DataGrid perfomanceSummaryDataGrid = new DataGrid();
                    perfomanceSummaryDataGrid.ItemsSource = performanceSummary;

                    Window perfomanceSummaryWindow = new Window();
                    perfomanceSummaryWindow.Title = $"отзывы о сотруднике {worker1.Name} {worker1.Surname} {worker1.Patronymic} (Сотрудник ID: {worker1.WorkerId}, Ресторан ID: {worker1.RestaurantId}) ";

                    perfomanceSummaryWindow.Content = perfomanceSummaryDataGrid;
                perfomanceSummaryWindow.Show();
            }           
            catch (Exception ex)
            {
                MessageBox.Show("произошла ошибка: " + ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}

