using bk654.Data;
using bk654.Models;
using bk654.WorkShiftFolder;
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

namespace bk654.RestaurantFolder
{
    public partial class ViewRestaurant : Window
    {
        private ApplicationContext dbContext;
        private List<Restaurant> _restaurants;
        private int _currentPage = 1;
        private int _pageSize;
        private int _totalPages;
        public ViewRestaurant()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadData(_currentPage, "worker_id");
        }

        private CreateRestaurant createRestaurant;
        private bool createRestaurantOpen = false;

        public void LoadData(int pageNumber, string sortBy, string searchCriteria = "")
        {
            try
            {
                IQueryable<Restaurant> query = dbContext.Restaurants;
                if (!string.IsNullOrWhiteSpace(searchCriteria))
                { 
                 var keywords = searchCriteria.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyword in keywords)
                    {
                        query = query.Where(w=>w.RestaurantCode.ToLower().Contains(keyword) || w.Mall.ToLower().Contains(keyword)|| w.RestaurantId.Equals(keyword));
                    }
                }
                int totalRecords = query.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);
                int offset = (pageNumber - 1 ) * _pageSize;   

                _restaurants = query.OrderBy(w=> w.RestaurantId).Skip(offset).Take(_pageSize).ToList();
                dataGrid.ItemsSource = _restaurants;
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
                LoadData(_currentPage, "worker_id");
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            LoadData(_currentPage, "worker_id");
        }
        private void ApplyRecordsPerPage_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(recordsPerPageTextBox.Text, out int recordsPerPage))
            {
                _pageSize = recordsPerPage;
                _currentPage = 1;
                LoadData(_currentPage, "worker_id");
            }
        }
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            LoadData(_currentPage, "worker_id");
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _totalPages;
            LoadData(_currentPage, "worker_id");
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = searchTextBox.Text.Replace("*", "").Trim();
            _currentPage = 1;
            LoadData(_currentPage, "worker_id", searchCriteria);
        }

        private Restaurant _selectedRestaurant;
        private void UpdateRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var selectedRow = (button.DataContext as dynamic); // Получаем выбранную строку

                // Сохраняем выбранный ресторан
                _selectedRestaurant = new Restaurant
                {
                    RestaurantCode = selectedRow.RestaurantCode,
                    Town = selectedRow.Town,
                    Address = selectedRow.Address,
                    Mall = selectedRow.Mall
                    // Дополнительные поля смены
                };

                // Открываем окно для редактирования смены
                UpdateRestaurant editRestaurantWindow = new UpdateRestaurant(_selectedRestaurant);
                editRestaurantWindow.ShowDialog();
            }
            LoadData(_currentPage, "worker_id");
        }

        public void DeleteRestaurant(int restaurantId)
        {
            Restaurant restaurantToDelete = dbContext.Restaurants.FirstOrDefault(w => w.RestaurantId == restaurantId);
            if (restaurantToDelete != null)
            {
                dbContext.Remove(restaurantToDelete);
                dbContext.SaveChanges();

                MessageBox.Show("ресторан успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("ресторан не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadData(_currentPage, "worker_id");
        }

        private void DeleteShiftButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedRow = (button.DataContext as dynamic);
            int restaurantId = selectedRow.RestaurantId;
            DeleteRestaurant(restaurantId);
            dataGrid.Items.Refresh();
            LoadData(_currentPage, "worker_id");
        }

        private void AddRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            if (!createRestaurantOpen)
            {
                createRestaurant = new CreateRestaurant();
                createRestaurant.Closed += (s, args) => createRestaurantOpen = false;
                createRestaurantOpen = true;
                createRestaurant.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void ShowWorkerinThisRestaurant_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Restaurant selectedRestaurant)
            {
                var selectedWorker = dbContext.Workers.FirstOrDefault(r => r.RestaurantId == selectedRestaurant.RestaurantId);
                if (selectedWorker != null)
                {
                    ShowWorkedInThisRestaurant(selectedRestaurant, selectedWorker);
                }
                else
                {
                    MessageBox.Show("Отзывы о выбранном сотруднике не найдены.", "Отсутствует информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Invalid selection. Please select a worker.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowWorkedInThisRestaurant(Restaurant selectedRestaurant, Worker selectedWorker)
        {
            try
            {
                if (selectedWorker != null && selectedRestaurant != null)
                {
                    var workersInRestaurant = dbContext.Workers
                        .Where(w => w.RestaurantId == selectedRestaurant.RestaurantId)
                        .Select(w => new
                        {
                            WorkerId = w.WorkerId,
                            PositionId = w.PositionId,
                            Surname = w.Surname,
                            Name = w.Name,
                            Patronymic = w.Patronymic,
                            StartDate = w.StartDate,
                            EndDate = w.EndDate,
                            DismissalReason = w.DismissalReason
                        })
                        .ToList();

                    DataGrid workersInRestaurantDataGrid = new DataGrid();
                    workersInRestaurantDataGrid.ItemsSource = workersInRestaurant;

                    Window workersInRestaurantWindow = new Window();
                    workersInRestaurantWindow.Title = $"работников в ресторане {selectedRestaurant.RestaurantId},{selectedRestaurant.Town}," +
                        $"{selectedRestaurant.Address},{selectedRestaurant.Mall},{selectedRestaurant.EmployeesCount}";

                    workersInRestaurantWindow.Content = workersInRestaurantDataGrid;
                    workersInRestaurantWindow.Show();
                }
                else
                {
                    MessageBox.Show("Invalid selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
