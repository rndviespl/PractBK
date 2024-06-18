using bk654.Data;
using bk654.Models;
using bk654.WorkerFolder;
using System.Windows;
using System.Windows.Controls;

namespace bk654.WorkShiftFolder
{
    /// <summary>
    /// Логика взаимодействия для ViewWorkerShift.xaml
    /// </summary>
    public partial class ViewWorkerShift : Window
    {
        private ApplicationContext dbContext;
        private List<WorkShift> _workShifts;
        private List<Worker> _workers;
        private int _currentPage = 1;
        private int _pageSize;
        private int _totalPages;

        public ViewWorkerShift()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadDataWorker(searchTextBox.Text);
        }

        private Window3 createWorkShiftWindow;
        private bool createWorkShiftWindowOpen = false;

        public void LoadDataWorker(string searchCriteria)
        {
            try
            {
                IQueryable<Worker> matchedWorkers = dbContext.Workers;

                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    var keywords = searchCriteria.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var keyword in keywords)
                    {
                        string keywordLower = keyword.ToLower();

                        matchedWorkers = matchedWorkers.Where(worker =>
                            worker.Surname.ToLower().Contains(keywordLower) ||
                            worker.Name.ToLower().Contains(keywordLower)
                        // || worker.WorkerId.Equals(int.Parse(keywordLower))
                        );
                    }
                }

                List<int> matchedWorkerIds = matchedWorkers.Select(worker => worker.WorkerId).ToList();

                IQueryable<WorkShift> query = dbContext.WorkShifts
                    .Where(ws => matchedWorkerIds.Contains(ws.WorkerId));

                int totalRecords = query.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);

                int offset = (_currentPage - 1) * _pageSize;
                _workers = dbContext.Workers.ToList();
                _workShifts = query.OrderBy(ws => ws.WorkShiftId).Skip(offset).Take(_pageSize).ToList();
                var displayItems = _workShifts.Select(workShift =>
                {
                    var worker = _workers.FirstOrDefault(w => w.WorkerId == workShift.WorkerId);

                    return new
                    {
                        workShift.WorkerId,
                        worker.PositionId,
                        worker.RestaurantId,
                        worker.Surname,
                        worker.Name,
                        worker.Patronymic,
                        workShift.WorkShiftId,
                        workShift.StartShift,
                        workShift.EndShift,
                        workShift.DescriptionManualEntry
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
                    LoadDataWorker(searchTextBox.Text);
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
                    LoadDataWorker(searchTextBox.Text);
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
                
                    // Проверяем корректность ввода номера страницы
                    if (int.TryParse(currentPageTextBox.Text, out int currentPage))
                    {
                        _currentPage = currentPage;
                        LoadDataWorker(searchTextBox.Text);
                    }
                    else
                    {
                        _currentPage = 1;
                        LoadDataWorker(searchTextBox.Text);
                    }
                 dataGrid.Items.Refresh();
               
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentPage = 1;
                LoadDataWorker(searchTextBox.Text);
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
                LoadDataWorker(searchTextBox.Text);
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
            LoadDataWorker(searchTextBox.Text);
        }
        private WorkShift selectedWorkShift;


        private void UpdateShiftButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var selectedRow = (button.DataContext as dynamic); // Получаем выбранную строку

                // Сохраняем выбранную смену
                selectedWorkShift = new WorkShift
                {
                    WorkShiftId = selectedRow.WorkShiftId,
                    StartShift = selectedRow.StartShift,
                    EndShift = selectedRow.EndShift,
                    DescriptionManualEntry = selectedRow.DescriptionManualEntry
                    // Дополнительные поля смены
                };

                // Открываем окно для редактирования смены
                WorkShiftUpdate editWorkShiftWindow = new WorkShiftUpdate(selectedWorkShift);
                editWorkShiftWindow.ShowDialog();
            }
        }

        public void DeleteWorkShift(int workShiftId)
        {
            WorkShift workShiftToDelete = dbContext.WorkShifts.FirstOrDefault(w => w.WorkShiftId == workShiftId);
            if (workShiftToDelete != null)
            {
                dbContext.Remove(workShiftToDelete);
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
            int workShiftId = selectedRow.WorkShiftId;
            DeleteWorkShift(workShiftId);
            dataGrid.Items.Refresh();
        }
       
        private void СreateWorkShift_Click(object sender, RoutedEventArgs e)
        {
            if (!createWorkShiftWindowOpen)
            {
                createWorkShiftWindow = new Window3();
                createWorkShiftWindow.Closed += (s, args) => createWorkShiftWindowOpen = false;
                createWorkShiftWindowOpen = true;
                createWorkShiftWindow.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
    }

}

     

