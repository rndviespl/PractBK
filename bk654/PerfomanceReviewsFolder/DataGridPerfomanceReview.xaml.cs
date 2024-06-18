using bk654.Data;
using bk654.Models;
using System.Windows;
using System.Windows.Controls;

namespace bk654.PerfomanceReviewsFolder
{
    /// <summary>
    /// Логика взаимодействия для DataGridPerfomanceReview.xaml
    /// </summary>
    public partial class DataGridPerfomanceReview : Window
    {
        private ApplicationContext dbContext;
        private List<Worker> _workers;
        private List<PerformanceReview> _performanceReviews;
        private int _currentPage = 1;
        private int _pageSize;
        private int _totalPages;
        public DataGridPerfomanceReview()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadDataReview(searchTextBox.Text);
        }
        private CreatePerfomanceReview createPerfomanceReview;
        private bool createPerfomanceReviewOpen = false;


        private void LoadDataReview(string searchCriteria)
        {
            try
            {
                IQueryable<PerformanceReview> matchedReview = dbContext.PerformanceReviews;
                IQueryable<Worker> matchedWorkers = dbContext.Workers;
                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    var keyWords = searchCriteria.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyWord in keyWords)
                    {
                        matchedWorkers = matchedWorkers.Where(worker =>
                            worker.Surname.ToLower().Contains(keyWord) ||
                            worker.Name.ToLower().Contains(keyWord)
                        );
                    }
                }
                List<int> matchedReviewId = matchedReview.Select(r => r.ReviewId).ToList();
                IQueryable<PerformanceReview> query = dbContext.PerformanceReviews.Where(w => matchedReviewId.Contains(w.ReviewId));
                int totalRecords = query.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
                int offset = (_currentPage - 1) * _pageSize;
                _workers = dbContext.Workers.ToList();
                _performanceReviews = query.OrderBy(w => w.ReviewId).Skip(offset).Take(_pageSize).ToList();
                var displayItems = _performanceReviews.Select(performanceReview =>
                {
                    var worker = _workers.FirstOrDefault(w => w.WorkerId == performanceReview.WorkerId);

                    return new
                    {
                        performanceReview.ReviewId,
                        performanceReview.WorkerId,
                        worker.PositionId,
                        worker.RestaurantId,
                        worker.Surname,
                        worker.Name,
                        worker.Patronymic,
                        performanceReview.ReviewerName,
                        performanceReview.ReviewDate,
                        performanceReview.PerformanceRating,
                        performanceReview.Comments
                    };
                }).ToList();

                dataGrid.ItemsSource = displayItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    LoadDataReview(searchTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentPage < _totalPages)
                {
                    _currentPage++;
                    LoadDataReview(searchTextBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyRecordsPerPage_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(recordsPerPageTextBox.Text, out int recordsPerPage))
            {
                _pageSize = recordsPerPage;


                _currentPage = 1;
                LoadDataReview(searchTextBox.Text);

                dataGrid.Items.Refresh();

            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentPage = 1;
                LoadDataReview(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentPage = _totalPages;
                LoadDataReview(searchTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = searchTextBox.Text.Replace("*", "").Trim();
            _currentPage = 1;
            LoadDataReview(searchTextBox.Text);
        }

        public void DeleteWorkShift(int reviewId)
        {
            PerformanceReview perfomanceToDelete = dbContext.PerformanceReviews.FirstOrDefault(w => w.ReviewId == reviewId);
            if (perfomanceToDelete != null)
            {
                dbContext.Remove(perfomanceToDelete);
                dbContext.SaveChanges();

                MessageBox.Show("смена работника успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("смена работника не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteShiftButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedRow = (button.DataContext as dynamic);
            int reviewId = selectedRow.WorkShiftId;
            DeleteWorkShift(reviewId);
            dataGrid.Items.Refresh();
        }
        private void ToСreatePerfomanceReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!createPerfomanceReviewOpen)
            {
                createPerfomanceReview = new CreatePerfomanceReview();
                createPerfomanceReview.Closed += (s, args) => createPerfomanceReviewOpen = false;
                createPerfomanceReviewOpen = true;
                createPerfomanceReview.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
    }
}
