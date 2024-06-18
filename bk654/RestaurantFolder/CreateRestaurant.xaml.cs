using bk654.Data;
using bk654.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace bk654.RestaurantFolder
{
    /// <summary>
    /// Логика взаимодействия для CreateRestaurant.xaml
    /// </summary>
    public partial class CreateRestaurant : Window
    {
        ApplicationContext dbContext;
        private List<Restaurant> _restaurants;
        public CreateRestaurant()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();

        }

        private void addRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            Restaurant restaurant = new Restaurant
            {
                RestaurantCode = addRestaurantCodeTextBox.Text,
                Address = addRestaurantAddressTextBox.Text,
                Mall = addRestaurantMallTextBox.Text,
                Town = addRestaurantTowntTextBox.Text,
            };

            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();

            try
            {
                MessageBox.Show("ресторан успешно добавлен", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
