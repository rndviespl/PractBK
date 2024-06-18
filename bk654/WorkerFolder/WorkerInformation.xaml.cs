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

namespace bk654.WorkerFolder
{
    /// <summary>
    /// Логика взаимодействия для WorkerInformation.xaml
    /// </summary>
    public partial class WorkerInformation : Window
    {
        public WorkerInformation()
        {
            InitializeComponent();
        }

        private UpdateWorker updateWorker;
        private bool updateWorkerOpen = false;
        private Window2 worker;
        private bool workerOpen = false;
        private DeleteWorkerWindow deleteWorkerWindow;
        private bool deleteWorkerWindowOpen = false;
        private ViewWorker viewWorkerWindow;
        private bool viewWorkerWindowOpen = false;

        private void OpenUpdateWorker_Click(object sender, RoutedEventArgs e)
        {
            if (!updateWorkerOpen)
            {
                updateWorker = new UpdateWorker();
                updateWorker.Closed += (s, args) => updateWorkerOpen = false;
                updateWorkerOpen = true;
                updateWorker.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void OpenCreateWorker_Click(object sender, RoutedEventArgs e)
        {
            if (!workerOpen)
            {
                worker = new Window2();
                worker.Closed += (s, args) => workerOpen = false;
                workerOpen = true;
                worker.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void OpenDeleteWorker_Click(object sender, RoutedEventArgs e)
        {
            if (!deleteWorkerWindowOpen)
            {
                deleteWorkerWindow = new DeleteWorkerWindow();
                deleteWorkerWindow.Closed += (s, args) => deleteWorkerWindowOpen = false;
                deleteWorkerWindowOpen = true;
                deleteWorkerWindow.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }

        private void OpenViewWorker_Click(object sender, RoutedEventArgs e)
        {
            if (!viewWorkerWindowOpen)
            {
                viewWorkerWindow = new ViewWorker();
                viewWorkerWindow.Closed += (s, args) => viewWorkerWindowOpen = false;
                viewWorkerWindowOpen = true;
                viewWorkerWindow.Show();
            }
            else
            {
                MessageBox.Show("Окно уже открыто!");
            }
        }
    }
}
