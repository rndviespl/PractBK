using bk654.Data;
using bk654.Models;
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
                // Получаем список всех работников
                var workers = dbContext.Workers.AsQueryable();

                // Фильтруем работников по критериям поиска, если они заданы
                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    var keywords = searchCriteria.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyword in keywords)
                    {
                        workers = workers.Where(worker =>
                            worker.Surname.ToLower().Contains(keyword) ||
                            worker.Name.ToLower().Contains(keyword));
                    }
                }
                // Получаем идентификаторы отобранных работников
                var workerIds = workers.Select(worker => worker.WorkerId).ToList();

                // Получаем смены этих работников
                var query = dbContext.WorkShifts.Where(ws => workerIds.Contains(ws.WorkerId));

                // Рассчитываем общее количество записей и страницы
                var totalRecords = query.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);

                // Вычисляем смещение и извлекаем текущую страницу смен
                var offset = (_currentPage - 1) * _pageSize;
                var workShifts = query.OrderBy(ws => ws.WorkShiftId).Skip(offset).Take(_pageSize).ToList();

                // Создаем список элементов для отображения
                var displayItems = workShifts.Select(workShift =>
                {
                    var worker = workers.FirstOrDefault(w => w.WorkerId == workShift.WorkerId);
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

                // Устанавливаем источник данных для таблицы
                dataGrid.ItemsSource = displayItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
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
                    };

                    // Открываем окно для редактирования смены
                    WorkShiftUpdate editWorkShiftWindow = new WorkShiftUpdate(selectedWorkShift);
                    editWorkShiftWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void DeleteWorkShift(int workShiftId)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteShiftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedRow = (button.DataContext as dynamic);
                int workShiftId = selectedRow.WorkShiftId;
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить смену у работника?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteWorkShift(workShiftId);
                }
                
                dataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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



