using bk654.Data;
using bk654.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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

namespace bk654.PositionAtWorkFolder
{
    /// <summary>
    /// Логика взаимодействия для DataTablePosAtWork.xaml
    /// </summary>
    public partial class DataTablePosAtWork : Window
    {
        private ApplicationContext dbContext;
        private List<PositionAtWork> _positionAtWorks;
        public DataTablePosAtWork()
        {
            InitializeComponent();
            dbContext = new ApplicationContext();
            LoadData();
        }
        private Window1 positionAtWork;
        private bool positionAtWorkOpen = false;
        private UpdatePos updatePos;
        private bool updatePosOpen = false;
        private DeletePositionWindow deletePositionWindow;
        private bool deletePositionWindowOpen = false;
        private void LoadData()
        {
            try
            {
               _positionAtWorks = dbContext.PositionAtWorks.FromSqlRaw("SELECT * FROM position_at_work").ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dataGrid.ItemsSource = _positionAtWorks;
        }


       

        private void PositionAtWork_Click(object sender, RoutedEventArgs e)
        {
            if (!positionAtWorkOpen)
            {
                positionAtWork = new Window1();
                positionAtWork.Closed += (s, args) => positionAtWorkOpen = false;
                positionAtWorkOpen = true;
                positionAtWork.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void UpdatePosButton_Click(object sender, RoutedEventArgs e)
        {
            if (!updatePosOpen)
            {
                updatePos = new UpdatePos();
                updatePos.Closed += (s, args) => updatePosOpen = false;
                updatePosOpen = true;
                updatePos.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void DeletePositionAtWorkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!deletePositionWindowOpen)
            {
                deletePositionWindow = new DeletePositionWindow();
                deletePositionWindow.Closed += (s, args) => deletePositionWindowOpen = false;
                deletePositionWindowOpen = true;
                deletePositionWindow.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
    }
}
