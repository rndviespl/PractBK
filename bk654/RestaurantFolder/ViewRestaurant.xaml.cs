﻿using bk654.Data;
using bk654.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace bk654.RestaurantFolder
{
    public partial class ViewRestaurant : Window
    {
        private ApplicationContext dbContext;
        private List<Restaurant> _restaurants;
        private List<Worker> _workers;
        private int _currentPage = 1;
        private int _pageSize;
        private int _totalPages;
        public ViewRestaurant()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadDataRestaurant(_currentPage, "worker_id");
        }

        private CreateRestaurant createRestaurant;
        private bool createRestaurantOpen = false;

        public void LoadDataRestaurant(int pageNumber, string sortBy, string searchCriteria = "")
        {
            try
            {
                // Подготавливаем запрос с предварительной загрузкой работников
                IQueryable<Restaurant> restaurantsQuery = dbContext.Restaurants.Include(r => r.Workers);
                // Фильтрация по критериям поиска
                if (!string.IsNullOrWhiteSpace(searchCriteria))
                {
                    var keywords = searchCriteria.ToLower().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyword in keywords)
                    {
                        restaurantsQuery = restaurantsQuery.Where(r =>
                             r.RestaurantCode.ToLower().Contains(keyword) ||
                             r.Mall.ToLower().Contains(keyword) || r.RestaurantId.ToString().Contains(keyword));
                    }
                }// Подсчитываем общее количество записей
                int totalRecords = restaurantsQuery.Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / _pageSize);

                // Вычисляем смещение и извлекаем текущую страницу ресторанов
                int offset = (pageNumber - 1) * _pageSize;
                var restaurants = restaurantsQuery.OrderBy(r => r.RestaurantId).Skip(offset).Take(_pageSize).ToList();
                // Устанавливаем источник данных для таблицы
                dataGrid.ItemsSource = restaurants;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadDataRestaurant(_currentPage, "worker_id");
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            LoadDataRestaurant(_currentPage, "worker_id");
        }
        private void ApplyRecordsPerPage_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(recordsPerPageTextBox.Text, out int recordsPerPage))
            {
                _pageSize = recordsPerPage;
                _currentPage = 1;
                LoadDataRestaurant(_currentPage, "worker_id");
            }
        }
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            LoadDataRestaurant(_currentPage, "worker_id");
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _totalPages;
            LoadDataRestaurant(_currentPage, "worker_id");
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchCriteria = searchTextBox.Text.Replace("*", "").Trim();
            _currentPage = 1;
            LoadDataRestaurant(_currentPage, "worker_id", searchCriteria);
        }

        private Restaurant _selectedRestaurant;
        private void UpdateRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        Mall = selectedRow.Mall,
                        // Дополнительные поля смены
                    };

                    // Открываем окно для редактирования смены
                    UpdateRestaurant editRestaurantWindow = new UpdateRestaurant(_selectedRestaurant);
                    editRestaurantWindow.ShowDialog();
                }
                LoadDataRestaurant(_currentPage, "worker_id");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void DeleteRestaurant(int restaurantId)
        {
            try
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
                LoadDataRestaurant(_currentPage, "worker_id");
            }
            catch (Exception ex)
            {

                MessageBox.Show("у вашей роли нет такой привелегии", "ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteShiftButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var selectedRow = (button.DataContext as dynamic);
                int restaurantId = selectedRow.RestaurantId;
                DeleteRestaurant(restaurantId);
                dataGrid.Items.Refresh();
                LoadDataRestaurant(_currentPage, "worker_id");
            }
            catch (Exception ex)
            {

                MessageBox.Show("у вашей роли нет такой привелегии", "ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    MessageBox.Show("сотрудники в выбранном ресторане не найдены.", "Отсутствует информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Invalid selection. Please select a restaurant.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            StartDate = w.StartDate
                        })
                        .ToList();

                    DataGrid workersInRestaurantDataGrid = new DataGrid();
                    workersInRestaurantDataGrid.ItemsSource = workersInRestaurant;

                    Window workersInRestaurantWindow = new Window();
                    workersInRestaurantWindow.Title = $"работников в ресторане {selectedRestaurant.RestaurantId},{selectedRestaurant.Town}," +
                        $"{selectedRestaurant.Address},{selectedRestaurant.Mall}, количество: {selectedRestaurant.WorkersCount} ";

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


