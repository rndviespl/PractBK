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

namespace bk654.RestaurantFolder
{
    /// <summary>
    /// Логика взаимодействия для UpdateRestaurant.xaml
    /// </summary>
    public partial class UpdateRestaurant : Window
    {
        ApplicationContext dbContext;
        private Restaurant selectedRestaurant;
        
        public UpdateRestaurant(Restaurant restaurant)
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            selectedRestaurant = restaurant;

            restaurantCodeTextBox.Text = selectedRestaurant.RestaurantCode;
            restaurantTowntTextBox.Text = selectedRestaurant.Town;
            restaurantAddressTextBox.Text = selectedRestaurant.Address;
            restaurantMallTextBox.Text = selectedRestaurant.Mall;

        }

        private void UpdateRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            selectedRestaurant.RestaurantCode = restaurantCodeTextBox.Text;
            selectedRestaurant.Town = restaurantTowntTextBox.Text;
            selectedRestaurant.Address = restaurantAddressTextBox.Text;
            selectedRestaurant.Mall = restaurantMallTextBox.Text;
            UpdateRestaurantDataSource(selectedRestaurant);
            MessageBox.Show("Запись успешно обновлена!");

            this.Close();
        }

        private void UpdateRestaurantDataSource(Restaurant Restaurant)
        {
        var existingRestaurant = dbContext.Restaurants.FirstOrDefault(w => w.RestaurantId == w.RestaurantId);
            if (existingRestaurant != null) 
            {
                existingRestaurant.RestaurantCode = selectedRestaurant.RestaurantCode;
                existingRestaurant.Town = selectedRestaurant.Town;
                existingRestaurant.Mall = selectedRestaurant.Mall;
                existingRestaurant.Address = selectedRestaurant.Address;
            }
            dbContext.SaveChanges();
        }
    }
}
